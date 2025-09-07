using BL.Models;

namespace BL.Api
{
    public interface IBlDataSourceType
    {
        Task<List<BlTDataSourceType>> GetAll();
        Task<BlTDataSourceType> GetById(int id);
        Task<BlTDataSourceType> Create(BlTDataSourceType item);
        Task<BlTDataSourceType> Update(BlTDataSourceType item);
        Task Delete(int id);
    }
}
