using BL.Api;
using Dal.Api;
using Dal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using BL.Models;

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

        public async Task<BlDashboardStatus> GetStatusCountsAsync()
        {
            var groups = await _dalDashboard.GetStatusCountsAsync() ?? new List<StatusCountDto>();

            var waiting = groups.Where(x => x.ImportStatusDesc == "pending" || x.ImportStatusDesc == "ממתין לקליטה").Sum(x => x.Count);
            var inProgress = groups.Where(x => x.ImportStatusDesc == "in-progress" || x.ImportStatusDesc == "בתהליך קליטה").Sum(x => x.Count);
            var success = groups.Where(x => x.ImportStatusDesc == "success" || x.ImportStatusDesc == "קליטה הסתיימה בהצלחה").Sum(x => x.Count);
            var error = groups.Where(x => x.ImportStatusDesc == "error" || x.ImportStatusDesc == "קליטה הסתיימה בכשלון").Sum(x => x.Count);
            var total = groups.Sum(x => x.Count);
            var other = total - (waiting + inProgress + success + error);
            if (other < 0) other = 0;

            return new BlDashboardStatus(waiting, inProgress, success, error, other);
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
            return await _dalDashboard.GetFilteredImportDataAsync(importStatusId, importDataSourceId, systemId, importFromDate, importToDate);
        }

        /// <summary>
        /// Calculates the total number of rows and the data volume as a formatted string (GB or MB).
        /// </summary>
        /// <param name="filteredData">The filtered APP_ImportControl records.</param>
        /// <returns>A tuple containing the total rows and the formatted data volume.</returns>
        public (int totalRows, string dataVolumeFormatted) CalculateDataVolume(List<AppImportControl> filteredData)
        {
            int totalRows = filteredData.Sum(x => x.TotalRows ?? 0);

            Console.WriteLine($"TotalRows: {totalRows}");

            ulong totalBytes = 0;
            foreach (var record in filteredData)
            {
                int rows = record.TotalRows ?? 0;
                Console.WriteLine($"Processing record with TotalRows: {rows}");

                // Ensure no overflow occurs during calculation
                try
                {
                    totalBytes += checked((ulong)rows * (4 + 8 + 255)); // INT (4 bytes) + DATETIME (8 bytes) + VARCHAR(255)
                }
                catch (OverflowException)
                {
                    Console.WriteLine("Overflow occurred during totalBytes calculation.");
                    totalBytes = ulong.MaxValue;
                    break;
                }
            }

            Console.WriteLine($"TotalBytes calculated: {totalBytes}");

            double dataVolumeInGB = (double)totalBytes / Math.Pow(1024, 3);

            Console.WriteLine($"DataVolumeInGB calculated: {dataVolumeInGB}");

            string dataVolumeFormatted;
            if (dataVolumeInGB >= 1)
            {
                // Round up to one decimal place
                dataVolumeInGB = Math.Ceiling(dataVolumeInGB * 10) / 10;
                dataVolumeFormatted = $"{dataVolumeInGB:F1} GB";
            }
            else
            {
                double dataVolumeInMB = (double)totalBytes / Math.Pow(1024, 2);

                Console.WriteLine($"DataVolumeInMB calculated: {dataVolumeInMB}");

                // Round up to one decimal place
                dataVolumeInMB = Math.Ceiling(dataVolumeInMB * 10) / 10;
                dataVolumeFormatted = $"{dataVolumeInMB:F1} MB";
            }

            Console.WriteLine($"DataVolumeFormatted: {dataVolumeFormatted}");

            return (totalRows, dataVolumeFormatted);
        }
    }
}
