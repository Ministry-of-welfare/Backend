using Dal.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dal.Api
{
    public interface IDalImportProblem
    {
        Task<List<AppImportProblem>> GetErrorsByImportControlIdAsync(int importControlId);
    }
}