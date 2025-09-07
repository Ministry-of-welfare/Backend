using System.Collections.Generic;
using System.Threading.Tasks;
using Dal.Models;

namespace Dal.Api
{
    public interface IDalDataSourceType
    {
        Task<List<TDataSourceType>> GetAll();
        Task<TDataSourceType> GetByIdAsync(int id);
        Task Create(TDataSourceType entity);
        Task Update(TDataSourceType entity);
        Task Delete(int id);
    }
}
