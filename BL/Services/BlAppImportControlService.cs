using BL.Api;
using BL.Models;
using Dal.Api;

namespace BL.Services
{
    public class BlAppImportControlService : IBlAppImportControl
    {
        private readonly IDalAppImportControl _dal;

        public BlAppImportControlService(IDalAppImportControl dal)
        {
            _dal = dal;
        }

        public BlAppImportControl CastingAppImportControlFromBlToDal(BlAppImportControl? e)
        {
            throw new NotImplementedException();
        }

        public Task<BlAppImportControl> Create(BlAppImportControl item)
        {
            throw new NotImplementedException();
        }

        public Task Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<BlAppImportControl>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<BlAppImportControl> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<BlAppImportControl> Update(BlAppImportControl item)
        {
            throw new NotImplementedException();
        }
    }
}
