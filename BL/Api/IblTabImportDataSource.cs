using BL.Models;
using Dal.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BL.Api
{
    public interface IBlTabImportDataSource
    {
        Task<List<BlTabImportDataSource>> GetAll();
        Task<BlTabImportDataSource> GetById(int id);
        Task<BlTabImportDataSource> Create(BlTabImportDataSource item);
        Task<BlTabImportDataSource> Update(BlTabImportDataSource item);
        Task Delete(int id);
    }
}


  

