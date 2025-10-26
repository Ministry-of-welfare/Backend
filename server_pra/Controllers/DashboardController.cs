using BL.Api;
using Dal.Api;
using Dal.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
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

        // GET: api/Dashboard/top-errors
        [HttpGet("top-errors")]
        public async Task<ActionResult<List<TopErrorDto>>> GetTopErrors(
            [FromQuery] int? statusId = null,
            [FromQuery] int? importDataSourceId = null,
            [FromQuery] int? systemId = null,
            [FromQuery] DateTime? startDate = null,
            [FromQuery] DateTime? endDate = null)
        {
            try
            {
                var topErrors = await _blDashboardService.GetTopErrors(statusId, importDataSourceId, systemId, startDate, endDate);
                return Ok(topErrors);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
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
        //    return Ok(result);
        //}
        [HttpGet("statusCounts")]
        public async Task<IActionResult> GetStatusCounts()
            {
                // Handle exceptions and return error response
                return StatusCode(500, new { Message = "An error occurred while processing the request.", Error = ex.Message });
            }
            var result = await _blDashboardService.GetStatusCountsAsync();
            return Ok(result);
        }

    }
}
