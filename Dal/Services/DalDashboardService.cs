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
        private readonly AppDbContext _context;

        // Constructor to inject the database context
        public DalDashboardService(AppDbContext context)
        {
            _context = context;
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
            // Start building the query
            var query = _context.AppImportControls.AsQueryable();

            // Apply filters dynamically based on provided parameters
            if (importStatusId.HasValue)
            {
                query = query.Where(x => x.ImportStatusId == importStatusId.Value);
            }

            if (importDataSourceId.HasValue)
            {
                query = query.Where(x => x.ImportDataSourceId == importDataSourceId.Value);
            }

            if (systemId.HasValue)
            {
                query = query.Where(x => x.ImportDataSource.SystemId == systemId.Value);
            }

            if (importFromDate.HasValue)
            {
                query = query.Where(x => x.ImportStartDate >= importFromDate.Value);
            }

            if (importToDate.HasValue)
            {
                query = query.Where(x => x.ImportStartDate <= importToDate.Value);
            }

            // Execute the query and return the results
            return await query.ToListAsync();
        }
    }
}
