using BL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Api
{
    public interface IBlFileStatus
    {

        Task<List<BlTFileStatus>> GetAll();
        Task<BlTFileStatus> GetById(int id);
        Task<BlTFileStatus> Create(BlTFileStatus item);
        Task<BlTFileStatus> Update(BlTFileStatus item);
        Task Delete(int id);



    }
}
