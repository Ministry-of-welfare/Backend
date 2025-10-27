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

            // Use ImportStatusId (stable) instead of description text matching
            var waiting = groups.Where(g => g.ImportStatusId == 0).Sum(g => g.Count);      // ממתין
            var inProgress = groups.Where(g => g.ImportStatusId == 1).Sum(g => g.Count);   // בתהליך
            var success = groups.Where(g => g.ImportStatusId == 2).Sum(g => g.Count);      // הצלחה
            var error = groups.Where(g => g.ImportStatusId == 3).Sum(g => g.Count);        // כשלון

            var total = groups.Sum(g => g.Count);
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
        /// Returns (int totalRows, double dataVolumeInGB) to match the interface.
        /// </summary>
        public (int totalRows, double dataVolumeInGB) CalculateDataVolume(List<AppImportControl> filteredData)
        {
            if (filteredData == null) return (0, 0.0);

            // Sum total rows using long to avoid overflow, then clamp to int.MaxValue
            long totalRowsLong = filteredData.Sum(x => (long)(x.TotalRows ?? 0));
            int totalRows = totalRowsLong > int.MaxValue ? int.MaxValue : (int)totalRowsLong;

            // Calculate total bytes safely using ulong and checked arithmetic
            ulong totalBytes = 0UL;
            foreach (var record in filteredData)
            {
                long rows = record.TotalRows ?? 0;
                if (rows <= 0) continue;

                // bytes per row estimate
                ulong bytesPerRow = (ulong)(4 + 8 + 255);

                try
                {
                    checked
                    {
                        totalBytes += (ulong)rows * bytesPerRow;
                    }
                }
                catch (OverflowException)
                {
                    totalBytes = ulong.MaxValue;
                    break;
                }
            }

            double dataVolumeInGB = (double)totalBytes / Math.Pow(1024, 3);

            return (totalRows, dataVolumeInGB);
        }

        // Updated: count imports — default to today's imports when caller provides no date range.
        public async Task<int> GetImportsCountAsync(int? statusId = null, int? importDataSourceId = null,
            int? systemId = null, DateTime? startDate = null, DateTime? endDate = null)
        {
            // If caller didn't pass a date range, count only today's imports (ImportFromDate).
            if (!startDate.HasValue && !endDate.HasValue)
            {
                var from = DateTime.Today;
                var to = DateTime.Today.AddDays(1).AddTicks(-1);
                var todayData = await _dalDashboard.GetFilteredImportDataAsync(statusId, importDataSourceId, systemId, from, to);
                return todayData?.Count ?? 0;
            }

            // If caller passed a partial range, fill sensible defaults (use provided dates as ImportFromDate filters)
            DateTime fromDate = startDate ?? DateTime.MinValue;
            DateTime toDate = endDate ?? DateTime.MaxValue;

            var data = await _dalDashboard.GetFilteredImportDataAsync(statusId, importDataSourceId, systemId, fromDate, toDate);
            return data?.Count ?? 0;
        }

        // Updated: success rate - count success by status id (2) — more reliable than string matching
        public async Task<double> GetSuccessRateAsync(int? statusId = null, int? importDataSourceId = null,
            int? systemId = null, DateTime? startDate = null, DateTime? endDate = null)
        {
            var groups = await _dalDashboard.GetStatusCountsAsync(statusId, importDataSourceId, systemId, startDate, endDate) ?? new List<StatusCountDto>();
            var total = groups.Sum(x => x.Count);
            if (total == 0) return 0.0;

            // Count success by status id (2) — more reliable than string matching
            var success = groups.Where(g => g.ImportStatusId == 2).Sum(g => g.Count);

            double percent = (double)success * 100.0 / total;
            return Math.Round(percent, 1);
        }

        // Average processing time (unchanged)
        public async Task<double> GetAverageProcessingTimeMinutesAsync(int? statusId = null, int? importDataSourceId = null,
            int? systemId = null, DateTime? startDate = null, DateTime? endDate = null)
        {
            var data = await _dalDashboard.GetFilteredImportDataAsync(statusId, importDataSourceId, systemId, startDate, endDate);
            if (data == null || !data.Any()) return 0.0;

            var durations = data
                .Where(x => x.ImportFinishDate.HasValue)
                .Select(x => (x.ImportFinishDate.Value - x.ImportStartDate).TotalMinutes)
                .Where(d => d >= 0);

            if (!durations.Any()) return 0.0;

            return Math.Round(durations.Average(), 1);
        }

        // Returns imports whose ImportFromDate is today, querying DAL with a day range to avoid timezone/time-of-day issues.
        public async Task<List<AppImportControl>> GetTodaysImportDataAsync(int? importStatusId = null, int? importDataSourceId = null, int? systemId = null)
        {
            var from = DateTime.Today;
            var to = DateTime.Today.AddDays(1).AddTicks(-1); // include whole day
            var todayList = await _dalDashboard.GetFilteredImportDataAsync(importStatusId, importDataSourceId, systemId, from, to);
            return todayList ?? new List<AppImportControl>();
        }
    }
}