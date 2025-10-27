using BL.Api;
using BL.Models;
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

            

        }

        public async Task<BlDashboardStatus> GetStatusCountsAsync(int? statusId = null, int? importDataSourceId = null,
            int? systemId = null, DateTime? startDate = null, DateTime? endDate = null)
        {
            var groups = await _dalDashboard.GetStatusCountsAsync(statusId, importDataSourceId, systemId, startDate, endDate) ?? new List<StatusCountDto>();

            var waiting = groups.Where(x => string.Equals(x.ImportStatusDesc, "pending", StringComparison.OrdinalIgnoreCase) || x.ImportStatusDesc == "ממתין לקליטה").Sum(x => x.Count);
            var inProgress = groups.Where(x => string.Equals(x.ImportStatusDesc, "in-progress", StringComparison.OrdinalIgnoreCase) || x.ImportStatusDesc == "בתהליך קליטה").Sum(x => x.Count);
            var success = groups.Where(x => string.Equals(x.ImportStatusDesc, "success", StringComparison.OrdinalIgnoreCase) || x.ImportStatusDesc == "קליטה הסתיימה בהצלחה").Sum(x => x.Count);
            var error = groups.Where(x => string.Equals(x.ImportStatusDesc, "error", StringComparison.OrdinalIgnoreCase) || x.ImportStatusDesc == "קליטה הסתיימה בכשלון").Sum(x => x.Count);
            var total = groups.Sum(x => x.Count);
            var other = total - (waiting + inProgress + success + error);
            if (other < 0) other = 0;

            return new BlDashboardStatus(waiting, inProgress, success, error, other);
        }
        public int CountDuplicateRecords(List<AppImportControl> records)
        {
            return records
                .GroupBy(r => $"{r.FileName?.Trim().ToLower()}|{r.ImportFromDate:yyyy-MM-dd}|{r.TotalRows}")
                .Where(g => g.Count() > 1)
                .Sum(g => g.Count() - 1);
        }




        /// <summary>
        /// Retrieves filtered data from the APP_ImportControl table based on the provided parameters.
        /// </summary>
        public async Task<List<AppImportControl>> GetFilteredImportDataAsync(int? importStatusId, int? importDataSourceId, int? systemId, DateTime? importFromDate, DateTime? importToDate)
        {
            return await _dalDashboard.GetFilteredImportDataAsync(importStatusId, importDataSourceId, systemId, importFromDate, importToDate);
        }

        /// <summary>
        /// Calculates the total number of rows and the data volume in GB for the filtered records.
        /// This implementation is synchronous as defined by the interface; it blocks briefly to fetch status groups.
        /// </summary>
        public (int totalRows, double dataVolumeInGB) CalculateDataVolume(List<AppImportControl> filteredData)
        {
            if (filteredData == null) return (0, 0.0);

            int totalRows = filteredData.Sum(x => x.TotalRows ?? 0);

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
