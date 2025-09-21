using BL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Api
{
    public interface IBlimportControl
    {

        Task<List<BlAppImportControl>> GetAll();
        Task<BlAppImportControl> GetById(int id);
        Task<BlAppImportControl> Create(BlAppImportControl item);
        Task<BlAppImportControl> Update(BlAppImportControl item);
        Task Delete(int id);

    }
}
