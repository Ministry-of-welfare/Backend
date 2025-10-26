using BL.Api;
using Microsoft.AspNetCore.Mvc;
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

        //[HttpGet("statusCounts")]
        //public async Task<IActionResult> GetStatusCounts()
        //{
        //    // _db.ImportControls הוא ה־DbSet שמכיל שורות קליטה
        //    var q = _db.ImportControls
        //        .GroupBy(ic => ic.ImportStatus) // או שדה סטטוס רלוונטי
        //        .Select(g => new { Status = g.Key, Count = g.Count() });

        //    var groups = await q.ToListAsync();

        //    var result = new
        //    {
        //        waiting = groups.Where(x => x.Status == "pending" || x.Status == "ממתין").Sum(x => x.Count),
        //        inProgress = groups.Where(x => x.Status == "in-progress" || x.Status == "בתהליך").Sum(x => x.Count),
        //        success = groups.Where(x => x.Status == "success" || x.Status == "הצלחה").Sum(x => x.Count),
        //        error = groups.Where(x => x.Status == "error" || x.Status == "כישלון").Sum(x => x.Count),
        //        other = groups.Sum(x => x.Count) // או חישוב מותאם ל־other
        //    };

        //    return Ok(result);
        //}
        [HttpGet("statusCounts")]
        public async Task<IActionResult> GetStatusCounts()
        {
            var result = await _blDashboardService.GetStatusCountsAsync();
            return Ok(result);
        }

    }
}
