using BL.Services;
using BL.Models;

using Dal.Api;
using Dal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Api
{
    public interface IblDashboardService
    {
        Task<BlDashboardStatus> GetStatusCountsAsync();
        // Get top errors with filters: status, data source, system, start date, end date
        Task<List<TopErrorDto>> GetTopErrors(int? statusId = null, int? importDataSourceId = null, 
            int? systemId = null, DateTime? startDate = null, DateTime? endDate = null);
        /// <summary>
        /// Retrieves filtered data from the APP_ImportControl table based on the provided parameters.
        /// </summary>
        /// <param name="importStatusId">Filter by Import Status ID (optional).</param>
        /// <param name="importDataSourceId">Filter by Import Data Source ID (optional).</param>
        /// <param name="systemId">Filter by System ID (optional).</param>
        /// <param name="importFromDate">Filter by Import Start Date (optional).</param>
        /// <param name="importToDate">Filter by Import End Date (optional).</param>
        /// <returns>A list of filtered APP_ImportControl records.</returns>
        Task<List<AppImportControl>> GetFilteredImportDataAsync(int? importStatusId, int? importDataSourceId, int? systemId, DateTime? importFromDate, DateTime? importToDate);

        /// <summary>
        /// Calculates the total number of rows and the data volume in GB for the filtered records.
        /// </summary>
        /// <param name="filteredData">The filtered APP_ImportControl records.</param>
        /// <returns>A tuple containing the total rows and the data volume in GB.</returns>
        (int totalRows, double dataVolumeInGB) CalculateDataVolume(List<AppImportControl> filteredData);
    }
}
