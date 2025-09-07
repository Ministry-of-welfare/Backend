using Dal.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BL.Api
{
    public interface IBlTabImportDataSource
    {
        Task<List<blTabImportDataSource>> GetAll();
        Task<blTabImportDataSource> GetById(int id);
        Task<blTabImportDataSource> Create(blTabImportDataSource item);
        Task<blTabImportDataSource> Update(blTabImportDataSource item);
        Task Delete(int id);
    }
}


  

