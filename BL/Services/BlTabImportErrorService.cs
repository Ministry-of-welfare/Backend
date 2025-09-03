using BL.Api;
using BL.Models;
using Dal.Api;

namespace BL.Services
{
    public class BlTabImportErrorService : IBlTabImportError
    {
        private readonly IDalTabImportError _dal;

        public BlTabImportErrorService(IDalTabImportError dal)
        {
            _dal = dal;
        }

        public BlTabImportError CastingTabImportErrorFromBlToDal(BlTabImportError? e)
        {
            throw new NotImplementedException();
        }

        public Task<BlTabImportError> Create(BlTabImportError item)
        {
            throw new NotImplementedException();
        }

        public Task Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<BlTabImportError>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<BlTabImportError> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<BlTabImportError> Update(BlTabImportError item)
        {
            throw new NotImplementedException();
        }
    }
}

