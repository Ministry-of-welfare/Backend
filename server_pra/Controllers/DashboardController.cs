using BL.Api;
using Microsoft.AspNetCore.Mvc;

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

    }
}
