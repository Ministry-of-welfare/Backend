using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dal.Api
{
    public interface IDalSystem
    {
        Task<List<Dal.Models.System>> GetAll();
        Task<Dal.Models.System> GetByIdAsync(int id);
        Task Create(Dal.Models.System entity);
        Task Update(Dal.Models.System entity);
        Task Delete(int id);
    }
}
