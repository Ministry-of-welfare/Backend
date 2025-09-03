using BL.Api;
using BL.Models;
using Dal.Api;

namespace BL.Services
{
    public class BlAppImportProblemService : IBlAppImportProblem
    {
        private readonly IDalAppImportProblem _dal;

        public BlAppImportProblemService(IDalAppImportProblem dal)
        {
            _dal = dal;
        }

        public BlAppImportProblem CastingAppImportProblemFromBlToDal(BlAppImportProblem? e)
        {
            throw new NotImplementedException();
        }

        public Task<BlAppImportProblem> Create(BlAppImportProblem item)
        {
            throw new NotImplementedException();
        }

        public Task Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<BlAppImportProblem>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<BlAppImportProblem> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<BlAppImportProblem> Update(BlAppImportProblem item)
        {
            throw new NotImplementedException();
        }
    }
}
