using System.Collections.Generic;
using System.Threading.Tasks;
using Dal.Models;

namespace Dal.Api
{
    public interface IDalDataSourceType
    {
        Task<List<DataSourceType>> GetAll();
        Task<DataSourceType> GetByIdAsync(int id);
        Task Create(DataSourceType entity);
        Task Update(DataSourceType entity);
        Task Delete(int id);
    }
}
