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
    }
}
