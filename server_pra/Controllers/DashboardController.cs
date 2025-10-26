using BL.Api;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace server_pra.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardController : ControllerBase
    {
        private readonly IblDashboardService _blDashboardService;

        public DashboardController(IblDashboardService blDashboardService)
        {
            _blDashboardService = blDashboardService;
        }

        /// <summary>
        /// Endpoint to retrieve filtered data and calculate data volume.
        /// </summary>
        /// <param name="importStatusId">Filter by Import Status ID (optional).</param>
        /// <param name="importDataSourceId">Filter by Import Data Source ID (optional).</param>
        /// <param name="systemId">Filter by System ID (optional).</param>
        /// <param name="importFromDate">Filter by Import Start Date (optional).</param>
        /// <param name="importToDate">Filter by Import End Date (optional).</param>
        /// <returns>Filtered data and calculated data volume.</returns>
        [HttpGet("GetDashboardData")]
        public async Task<IActionResult> GetDashboardData(int? importStatusId, int? importDataSourceId, int? systemId, DateTime? importFromDate, DateTime? importToDate)
        {
            try
            {
                // Retrieve filtered data
                var filteredData = await _blDashboardService.GetFilteredImportDataAsync(importStatusId, importDataSourceId, systemId, importFromDate, importToDate);

                // Calculate data volume
                var (totalRows, dataVolumeInGB) = _blDashboardService.CalculateDataVolume(filteredData);

                // Return the result
                return Ok(new
                {
                    TotalRows = totalRows,
                    DataVolumeInGB = dataVolumeInGB
                });
            }
            catch (Exception ex)
            {
                // Handle exceptions and return error response
                return StatusCode(500, new { Message = "An error occurred while processing the request.", Error = ex.Message });
            }
        }
    }
}
