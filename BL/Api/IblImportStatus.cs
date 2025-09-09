using BL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Api
{
    public interface IBlImportStatus
    {
        Task<List<BlTImportStatus>> GetAll();
        Task<BlTImportStatus> GetById(int id);
        Task<BlTImportStatus> Create(BlTImportStatus item);
        Task<BlTImportStatus> Update(BlTImportStatus item);
        Task Delete(int id);
    }
}
