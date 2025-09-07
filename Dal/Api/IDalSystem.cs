using System.Collections.Generic;
using System.Threading.Tasks;
using Dal.Models;

namespace Dal.Api
{
    public interface IDalSystem
    {
        Task<List<System>> GetAll();
        Task<System> GetByIdAsync(int id);
        Task Create(System entity);
        Task Update(System entity);
        Task Delete(int id);
    }
}
