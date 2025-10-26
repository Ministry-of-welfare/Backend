using BL.Api;
using BL.Models;
using Dal.Api;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace BL.Services
{
    public class BlDashboardService : IblDashboardService
    {
        private readonly IdalDashboard _dal;

        public BlDashboardService(IdalDashboard dal)
        {
            _dal = dal;
        }

        public async Task<BlDashboardStatus> GetStatusCountsAsync()
        {
            var groups = await _dal.GetStatusCountsAsync() ?? new List<StatusCountDto>();

            var waiting = groups.Where(x => x.ImportStatusDesc == "pending" || x.ImportStatusDesc == "ממתין לקליטה").Sum(x => x.Count);
            var inProgress = groups.Where(x => x.ImportStatusDesc == "in-progress" || x.ImportStatusDesc == "בתהליך קליטה").Sum(x => x.Count);
            var success = groups.Where(x => x.ImportStatusDesc == "success" || x.ImportStatusDesc == "קליטה הסתיימה בהצלחה").Sum(x => x.Count);
            var error = groups.Where(x => x.ImportStatusDesc == "error" || x.ImportStatusDesc == "קליטה הסתיימה בכשלון").Sum(x => x.Count);

            var total = groups.Sum(x => x.Count);
            var other = total - (waiting + inProgress + success + error);
            if (other < 0) other = 0;

            return new BlDashboardStatus(waiting, inProgress, success, error, other);
        }
    }
}