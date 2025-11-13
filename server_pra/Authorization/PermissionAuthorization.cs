using Dal.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace server_pra.Authorization
{
    public class PermissionRequirement : IAuthorizationRequirement
    {
        public string Permission { get; }
        public string ResourceRouteKey { get; }
        public PermissionRequirement(string permission, string resourceRouteKey = "templateId")
        {
            Permission = permission;
            ResourceRouteKey = resourceRouteKey;
        }
    }

    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
    public class HasPermissionAttribute : AuthorizeAttribute
    {
        public HasPermissionAttribute(string permission) : base(policy: $"Permission:{permission}") { }
    }

    public class PermissionPolicyProvider : IAuthorizationPolicyProvider
    {
        private const string PREFIX = "Permission:";
        private readonly DefaultAuthorizationPolicyProvider _fallback;
        public PermissionPolicyProvider(Microsoft.Extensions.Options.IOptions<AuthorizationOptions> options)
        {
            _fallback = new DefaultAuthorizationPolicyProvider(options);
        }

        public Task<AuthorizationPolicy> GetDefaultPolicyAsync() => _fallback.GetDefaultPolicyAsync();
        public Task<AuthorizationPolicy> GetFallbackPolicyAsync() => _fallback.GetFallbackPolicyAsync();

        public Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
        {
            if (!string.IsNullOrEmpty(policyName) && policyName.StartsWith(PREFIX, StringComparison.OrdinalIgnoreCase))
            {
                var permission = policyName.Substring(PREFIX.Length);
                var policy = new AuthorizationPolicyBuilder()
                    .AddRequirements(new PermissionRequirement(permission))
                    .Build();
                return Task.FromResult(policy);
            }
            return _fallback.GetPolicyAsync(policyName);
        }
    }

    public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
    {
        private readonly AppDbContext _db;
        private readonly IHttpContextAccessor _http;
        public PermissionHandler(AppDbContext db, IHttpContextAccessor http)
        {
            _db = db;
            _http = http;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            var http = _http.HttpContext ?? (context.Resource as Microsoft.AspNetCore.Mvc.Filters.AuthorizationFilterContext)?.HttpContext;
            if (http == null) { context.Fail(); return; }

            var idClaim = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var nameClaim = context.User.Identity?.Name;
            var subClaim = context.User.FindFirst("sub")?.Value;
            string principal = idClaim ?? nameClaim ?? subClaim;
            if (string.IsNullOrEmpty(principal)) { context.Fail(); return; }

            // read resource id from route or query
            int? resourceId = null;
            if (http.Request.RouteValues.TryGetValue(requirement.ResourceRouteKey, out var rv) && rv != null)
            {
                if (int.TryParse(rv.ToString(), out var rid))
                    resourceId = rid;
            }
            if (!resourceId.HasValue && http.Request.Query.TryGetValue(requirement.ResourceRouteKey, out var qv))
            {
                if (int.TryParse(qv.ToString(), out var rid2))
                    resourceId = rid2;
            }

            bool HasFlag(TemplatePermission tp, string perm) =>
                perm switch
                {
                    "View" => tp.CanView,
                    "Edit" => tp.CanEdit,
                    "Delete" => tp.CanDelete,
                    "Duplicate" => tp.CanDuplicate,
                    _ => false
                };

            // If token NameIdentifier is numeric -> treat as UserId, otherwise as username/email
            int numericUserId = -1;
            var isNumericId = int.TryParse(principal, out numericUserId);

            if (resourceId.HasValue)
            {
                // 1) user-specific template permission
                if (isNumericId)
                {
                    var user = await _db.TabUsers.FirstOrDefaultAsync(u => u.UserId == numericUserId);
                    if (user != null)
                    {
                        var tpUser = await _db.TemplatePermissions
                            .Where(t => t.TemplateId == resourceId.Value && t.PrincipalType == "User" && (t.PrincipalName == user.UserName || t.PrincipalName == user.Email || t.PrincipalName == user.UserId.ToString()))
                            .FirstOrDefaultAsync();
                        if (tpUser != null && HasFlag(tpUser, requirement.Permission)) { context.Succeed(requirement); return; }
                    }
                }
                else
                {
                    var tpUser = await _db.TemplatePermissions
                        .Where(t => t.TemplateId == resourceId.Value && t.PrincipalType == "User" && t.PrincipalName == principal)
                        .FirstOrDefaultAsync();
                    if (tpUser != null && HasFlag(tpUser, requirement.Permission)) { context.Succeed(requirement); return; }
                }

                // 2) role checks: find user's active roles
                var userEntity = isNumericId
                    ? await _db.TabUsers.FirstOrDefaultAsync(u => u.UserId == numericUserId)
                    : await _db.TabUsers.FirstOrDefaultAsync(u => u.UserName == principal || u.Email == principal);

                if (userEntity != null)
                {
                    // use DateOnly for comparison to match DB date-only columns
                    var now = DateOnly.FromDateTime(DateTime.UtcNow);
                    var roleIds = await _db.TabUserRoles
                        .Where(ur => ur.UserId == userEntity.UserId && (ur.ToDate == null || ur.ToDate >= now) && (ur.FromDate == null || ur.FromDate <= now))
                        .Select(ur => ur.RoleId).ToListAsync();

                    if (roleIds.Any())
                    {
                        // template role entries
                        var roleNames = await _db.TabRoles.Where(r => roleIds.Contains(r.RoleId)).Select(r => r.RoleName).ToListAsync();
                        var tpRole = await _db.TemplatePermissions
                            .Where(t => t.TemplateId == resourceId.Value && t.PrincipalType == "Role" && roleNames.Contains(t.PrincipalName))
                            .FirstOrDefaultAsync();
                        if (tpRole != null && HasFlag(tpRole, requirement.Permission)) { context.Succeed(requirement); return; }

                        // role -> permission mapping
                        var roleHasPerm = await (from rp in _db.TabRolePermissions
                                                 join p in _db.TabPermissions on rp.PermissionId equals p.PermissionId
                                                 where roleIds.Contains(rp.RoleId) && p.PermissionName == requirement.Permission
                                                 select rp).AnyAsync();
                        if (roleHasPerm) { context.Succeed(requirement); return; }
                    }
                }

                context.Fail();
                return;
            }

            // Global permission: check role -> permission mapping
            var userGlobal = isNumericId
                ? await _db.TabUsers.FirstOrDefaultAsync(u => u.UserId == numericUserId)
                : await _db.TabUsers.FirstOrDefaultAsync(u => u.UserName == principal || u.Email == principal);

            if (userGlobal == null) { context.Fail(); return; }

            var nowGlobal = DateOnly.FromDateTime(DateTime.UtcNow);
            var userRoleIdsGlobal = await _db.TabUserRoles
                .Where(ur => ur.UserId == userGlobal.UserId && (ur.ToDate == null || ur.ToDate >= nowGlobal) && (ur.FromDate == null || ur.FromDate <= nowGlobal))
                .Select(ur => ur.RoleId).ToListAsync();

            if (userRoleIdsGlobal.Any())
            {
                var roleHasPermission = await (from rp in _db.TabRolePermissions
                                               join p in _db.TabPermissions on rp.PermissionId equals p.PermissionId
                                               where userRoleIdsGlobal.Contains(rp.RoleId) && p.PermissionName == requirement.Permission
                                               select rp).AnyAsync();
                if (roleHasPermission) { context.Succeed(requirement); return; }
            }

            context.Fail();
        }
    }
}