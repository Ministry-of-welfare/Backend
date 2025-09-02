using Dal.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dal.Api
{
    public interface IEnvironmentEntity
    {
        Task<IEnumerable<EnvironmentEntity>> GetAllAsync();
        Task<EnvironmentEntity?> GetByIdAsync(int id);
        Task AddAsync(EnvironmentEntity entity);
        Task UpdateAsync(EnvironmentEntity entity);
        Task DeleteAsync(int id);
    }
}
