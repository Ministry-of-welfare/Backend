using Dal.Api;
using Dal.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dal.Services
{
    public class DalImportProblemService : IDalImportProblem
    {
        private readonly AppDbContext _context;

        public DalImportProblemService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<AppImportProblem>> GetErrorsByImportControlIdAsync(int importControlId)
        {
            return await _context.AppImportProblems
                .Where(p => p.ImportControlId == importControlId)
                .ToListAsync();
        }
    }
}