using Dal.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using server_pra.Services;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace server_pra.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TabUserController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly JwtService _jwtService;

        public TabUserController(AppDbContext context, JwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        [HttpPost]
        public IActionResult Create(TabUser item)
        {
            _context.TabUsers.Add(item);
            _context.SaveChanges();
            return Ok(item);
        }

        [HttpGet]
        public IActionResult GetAll() => Ok(_context.TabUsers.Select(u => new {
            u.UserId,
            u.UserName,
            u.DisplayName,
            u.Email,
            u.IsActive
        }).ToList());

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var entity = _context.TabUsers.Find(id);
            return entity == null ? NotFound() : Ok(entity);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, TabUser item)
        {
            var entity = _context.TabUsers.Find(id);
            if (entity == null) return NotFound();

            entity.UserName = item.UserName;
            entity.Email = item.Email;
            entity.FirstName = item.FirstName;
            entity.LastName = item.LastName;
            entity.Password = item.Password;

            entity.IsActive = item.IsActive;
            entity.DisplayName = item.DisplayName;

            _context.SaveChanges();
            return Ok(entity);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var entity = _context.TabUsers.Find(id);
            if (entity == null) return NotFound();

            _context.TabUsers.Remove(entity);
            _context.SaveChanges();
            return Ok();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.UserName))
                return BadRequest(new { message = "שם משתמש נדרש" });

            if (string.IsNullOrWhiteSpace(request.Password))
                return BadRequest(new { message = "סיסמה נדרשת" });
            var user = await _context.TabUsers
                .Include(u => u.TabUserRoles)
                    .ThenInclude(ur => ur.Role)
                        .ThenInclude(r => r.TabRolePermissions)
                            .ThenInclude(rp => rp.Permission)
                .FirstOrDefaultAsync(u => u.UserName == request.UserName && u.IsActive);

            if (user == null || user.Password != request.Password)
                return Unauthorized(new { message = "שם משתמש או סיסמה שגויים" });

            var today = DateOnly.FromDateTime(DateTime.Now);
            
            var roles = user.TabUserRoles
                .Where(ur => ur.FromDate == null || ur.FromDate <= today)
                .Where(ur => ur.ToDate == null || ur.ToDate >= today)
                .Select(ur => ur.Role.RoleName)
                .ToList();

            var permissions = user.TabUserRoles
                .Where(ur => ur.FromDate == null || ur.FromDate <= today)
                .Where(ur => ur.ToDate == null || ur.ToDate >= today)
                .SelectMany(ur => ur.Role.TabRolePermissions)
                .Select(rp => rp.Permission.PermissionName)
                .Distinct()
                .ToList();

            var primaryRole = roles.FirstOrDefault() ?? "User";
            var token = _jwtService.GenerateToken(user.UserId.ToString(), primaryRole, permissions);

            return Ok(new
            {
                token,
                user = new
                {
                    user.UserId,
                    user.UserName,
                    user.DisplayName,
                    user.Email,
                    roles,
                    permissions
                }
            });
        }
    }

    public class LoginRequest
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
