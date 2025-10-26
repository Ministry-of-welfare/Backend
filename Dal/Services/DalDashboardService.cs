using Dal.Api;
using Dal.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dal.Services
{
    public class DalDashboardService : IdalDashboard
    {
        private readonly AppDbContext _context;

        public DalDashboardService(AppDbContext context)
        {
            _context = context;
        }

        // Group by ImportStatusId and return count of rows per status (include Description if exists)
        public async Task<List<StatusCountDto>> GetStatusCountsAsync()
        {
            var q = _context.AppImportControls
                            .Include(ic => ic.ImportStatus)
                            .GroupBy(ic => ic.ImportStatusId)
                            .Select(g => new StatusCountDto(
                                g.Key,
                                g.Select(x => x.ImportStatus != null ? x.ImportStatus.ImportStatusDesc : null).FirstOrDefault() ?? string.Empty,
                                g.Count()
                            ));

            return await q.ToListAsync();
        }
    }
}