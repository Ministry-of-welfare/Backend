using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dal.Api;
using Dal.Models;
using Microsoft.EntityFrameworkCore;

namespace Dal.Services
{
    public class DalValidationRuleService : IDalValidationRule // Ensure this interface is stable
    {
        private readonly AppDbContext _dbContext;

        public DalValidationRuleService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<TabValidationRule>> GetValidationRulesAsync(int importDataSourceId)
        {
            return await _dbContext.TabValidationRules
                .Where(rule => rule.ImportDataSourceId == importDataSourceId && rule.IsEnabled)
                .Include(rule => rule.Conditions)
                .Include(rule => rule.Asserts)
                .ToListAsync();
        }
    }
}
