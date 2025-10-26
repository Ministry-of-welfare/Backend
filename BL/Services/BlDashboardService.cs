
using BL.Api;
using Dal.Api;
using Dal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BL.Services
{
    public class BlDashboardService : IblDashboardService
    {
        private readonly IdalDashboard _dalDashboard;

        // Constructor to inject the DAL service
        public BlDashboardService(IdalDashboard dalDashboard)
        {
            _dalDashboard = dalDashboard;
        }

        // Get top errors with filters: status, data source, system, start date, end date
        public async Task<List<TopErrorDto>> GetTopErrors(int? statusId = null, int? importDataSourceId = null, 
            int? systemId = null, DateTime? startDate = null, DateTime? endDate = null)
        {
            return await _dalDashboard.GetTopErrors(statusId, importDataSourceId, systemId, startDate, endDate);
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
            return await _dalDashboard.GetFilteredImportDataAsync(importStatusId, importDataSourceId, systemId, importFromDate, importToDate);
        }

        /// <summary>
        /// Calculates the total number of rows and the data volume in GB for the filtered records.
        /// </summary>
        /// <param name="filteredData">The filtered APP_ImportControl records.</param>
        /// <returns>A tuple containing the total rows and the data volume in GB.</returns>
        public (int totalRows, double dataVolumeInGB) CalculateDataVolume(List<AppImportControl> filteredData)
        {
            int totalRows = filteredData.Sum(x => x.TotalRows ?? 0);

            // Example calculation for data volume based on field sizes
            long totalBytes = 0;
            foreach (var record in filteredData)
            {
                totalBytes += (record.TotalRows ?? 0) * (4 + 8 + 255); // INT (4 bytes) + DATETIME (8 bytes) + VARCHAR(255)
            }

            // Convert bytes to GB
            double dataVolumeInGB = totalBytes / Math.Pow(1024, 3);

            return (totalRows, dataVolumeInGB);
        }
    }
}
