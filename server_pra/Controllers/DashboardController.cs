using BL.Api;
using Dal.Api;
using Dal.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
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
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpGet("data-quality-simple")]
        public async Task<IActionResult> GetDataQualityKpisSimple(
    [FromQuery] int? importDataSourceId = null,
    [FromQuery] int? systemId = null)
        {
            try
            {
                // מביא את כל הרשומות עם או בלי פילטרים
                var records = await _blDashboardService.GetFilteredImportDataAsync(
                    null, importDataSourceId, systemId, null, null);

                // חישוב נתוני איכות נתונים לפי ImportStatusId וגם לפי ImportControlId
                var result = records
                    .GroupBy(r => new { r.ImportStatusId, r.ImportControlId })
                    .Select(g =>
                    {
                        var total = g.Sum(r => r.TotalRows ?? 0);
                        var invalid = g.Sum(r => r.RowsInvalid ?? 0);
                        var validPercent = total > 0 ? ((double)(total - invalid) / total) * 100 : 0;

                        return new
                        {
                            ImportStatusId = g.Key.ImportStatusId, // פשוט הורד את ה-??
                            ImportControlId = g.Key.ImportControlId,
                            TotalRows = total,
                            RowsInvalid = invalid,
                            ValidRowsPercentage = Math.Round(validPercent, 2)
                        };
                    })

                    .OrderBy(r => r.ImportStatusId)
                    .ThenBy(r => r.ImportControlId)
                    .ToList();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Message = "Failed to retrieve data quality KPIs.",
                    Error = ex.Message
                });
            }
        }

        /// <summary>
        /// GET: api/Dashboard/statusCounts
        /// </summary>
        [HttpGet("statusCounts")]
        public async Task<IActionResult> GetStatusCounts(
            [FromQuery] int? statusId = null,
            [FromQuery] int? importDataSourceId = null,
            [FromQuery] int? systemId = null,
            [FromQuery] DateTime? startDate = null,
            [FromQuery] DateTime? endDate = null)
        {
            try
            {
                var result = await _blDashboardService.GetStatusCountsAsync(statusId, importDataSourceId, systemId, startDate, endDate);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while processing the request.", Error = ex.Message });
            }
        }
    }
}
