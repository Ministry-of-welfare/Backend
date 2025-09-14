using BL.Models;
using Dal.Models;

namespace BL.Api
{
    public interface IblTabImportDataSourceColumn
    {
        Task<List<BlTabImportDataSourceColumn>> GetAll();
        Task<BlTabImportDataSourceColumn> GetById(int id);
        Task Create(BlTabImportDataSourceColumn item);
        Task<BlTabImportDataSourceColumn> Update(BlTabImportDataSourceColumn item);
        Task Delete(int id);
    }
}
