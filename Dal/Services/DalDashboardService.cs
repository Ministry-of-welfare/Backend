using Dal.Api;
using Dal.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
                .GroupBy(p => new { 
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
    }
}
