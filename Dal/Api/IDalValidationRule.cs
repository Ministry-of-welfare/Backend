using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dal.Models;

namespace Dal.Api
{
    public interface IDalValidationRule
    {
       Task<List<TabValidationRule>> GetValidationRulesAsync(int importDataSourceId);

    }
}
