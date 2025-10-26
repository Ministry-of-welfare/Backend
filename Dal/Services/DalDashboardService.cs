using Dal.Api;
using Dal.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;   
using System.Linq;
using System.Threading.Tasks;

namespace Dal.Services
{
    public class DalDashboardService : IdalDashboard
    {
        private readonly AppDbContext _db;

        public DalDashboardService(AppDbContext db)
        {
            _db = db;
        }

        // Group by ImportStatusId and return count of rows per status (include Description if exists)
        public async Task<List<StatusCountDto>> GetStatusCountsAsync(int? statusId = null, int? importDataSourceId = null,
            int? systemId = null, DateTime? startDate = null, DateTime? endDate = null)
        {
            var query = _db.AppImportControls
                           .Include(ic => ic.ImportStatus)
                           .Include(ic => ic.ImportDataSource)
                           .AsQueryable();

            // apply filters if provided
            if (statusId.HasValue)
                query = query.Where(ic => ic.ImportStatusId == statusId.Value);

            if (importDataSourceId.HasValue)
                query = query.Where(ic => ic.ImportDataSourceId == importDataSourceId.Value);

            if (systemId.HasValue)
                query = query.Where(ic => ic.ImportDataSource != null && ic.ImportDataSource.SystemId == systemId.Value);

            if (startDate.HasValue)
                query = query.Where(ic => ic.ImportStartDate >= startDate.Value);

            if (endDate.HasValue)
                query = query.Where(ic => ic.ImportStartDate <= endDate.Value);

            var q = query
                        .GroupBy(ic => ic.ImportStatusId)
                        .Select(g => new StatusCountDto(
                            g.Key,
                            g.Select(x => x.ImportStatus != null ? x.ImportStatus.ImportStatusDesc : null).FirstOrDefault() ?? string.Empty,
                            g.Count()
                        ));

            return await q.ToListAsync();
        }

        // Get top errors with filters: status, data source, system, start date, end date
        public async Task<List<TopErrorDto>> GetTopErrors(int? statusId = null, int? importDataSourceId = null,
            int? systemId = null, DateTime? startDate = null, DateTime? endDate = null)
        {
            var query = _db.AppImportProblems
                .Include(p => p.ImportControl)
                .ThenInclude(c => c.ImportDataSource)
                .Include(p => p.ImportError)
                .Where(p => p.ImportControl != null);

            // Apply filters
            if (statusId.HasValue)
                query = query.Where(p => p.ImportControl.ImportStatusId == statusId.Value);
            else
                query = query.Where(p => p.ImportControl.ImportStatusId == 3); // Default: failed imports

            if (importDataSourceId.HasValue)
                query = query.Where(p => p.ImportControl.ImportDataSourceId == importDataSourceId.Value);

            if (systemId.HasValue)
                query = query.Where(p => p.ImportControl.ImportDataSource.SystemId == systemId.Value);

            if (startDate.HasValue)
                query = query.Where(p => p.ImportControl.ImportStartDate >= startDate.Value);

            if (endDate.HasValue)
                query = query.Where(p => p.ImportControl.ImportStartDate <= endDate.Value);

            var topErrors = await query
                .GroupBy(p => new
                {
                    p.ImportErrorId,
                    ErrorDescription = p.ImportError != null ? p.ImportError.ImportErrorDesc : p.ErrorDetail
                })
                .Select(g => new TopErrorDto
                {
                    ImportErrorId = g.Key.ImportErrorId ?? 0,
                    ErrorColumn = g.First().ErrorColumn,
                    ErrorValue = g.First().ErrorValue,
                    ErrorDetail = g.Key.ErrorDescription,
                    ImportControlId = g.First().ImportControlId ?? 0,
                    ErrorCount = g.Count()
                })
                .OrderByDescending(e => e.ErrorCount)
                .ToListAsync();

            return topErrors;
        }

        /// <summary>
        /// Retrieves filtered data from the APP_ImportControl table based on the provided parameters.
        /// </summary>
        /// <param name="importStatusId">Filter by Import Status ID (optional).</param>
        /// <param name="importDataSourceId">Filter by Import Data Source ID (optional).</param>
        /// <param name="systemId">Filter by System ID (optional).</param>
        /// <param name="importFromDate">Filter by Import Start Date (optional).</param>
        /// <param name="importToDate">Filter by Import End Date (optional).</param>
        /// <returns>A list of filtered APP_ImportControl records.</returns>
        public async Task<List<AppImportControl>> GetFilteredImportDataAsync(int? importStatusId, int? importDataSourceId, int? systemId, DateTime? importFromDate, DateTime? importToDate)
        {
            var query = _db.AppImportControls.AsQueryable();

            if (importStatusId.HasValue)
                query = query.Where(x => x.ImportStatusId == importStatusId.Value);

            if (importDataSourceId.HasValue)
                query = query.Where(x => x.ImportDataSourceId == importDataSourceId.Value);

            if (systemId.HasValue)
                query = query.Where(x => x.ImportDataSource != null && x.ImportDataSource.SystemId == systemId.Value);

            if (importFromDate.HasValue)
                query = query.Where(x => x.ImportStartDate >= importFromDate.Value);

            if (importToDate.HasValue)
                query = query.Where(x => x.ImportStartDate <= importToDate.Value);

            return await query.ToListAsync();
        }
    }
}
