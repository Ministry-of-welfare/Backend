using BL.Models;

namespace BL.Api
{
    public interface IBlSystem
    {
        Task<List<BlTSystem>> GetAll();
        Task<BlTSystem> GetById(int id);
        Task<BlTSystem> Create(BlTSystem item);
        Task<BlTSystem> Update(BlTSystem item);
        Task Delete(int id);
        Task<IEnumerable<SystemPerformanceDto>> GetSystemPerformanceAsync();

    }
}
