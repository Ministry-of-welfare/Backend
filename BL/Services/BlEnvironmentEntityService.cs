using BL.Api;
using BL.Models;
using Dal.Api;

namespace BL.Services
{
    public class BlEnvironmentEntityService : IBlEnvironmentEntity
    {
        private readonly IDalEnvironment _dal; // Ensure IDalEnvironment is defined and accessible

        public BlEnvironmentEntityService(IDalEnvironment dal)
        {
            _dal = dal;
        }

        public BlEnvironmentEntity CastingEnvironmentEntityFromBlToDal(BlEnvironmentEntity? e)
        {
            throw new NotImplementedException();
        }

        public Task<BlEnvironmentEntity> Create(BlEnvironmentEntity item)
        {
            throw new NotImplementedException();
        }

        public Task Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<BlEnvironmentEntity>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<BlEnvironmentEntity> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<BlEnvironmentEntity> Update(BlEnvironmentEntity item)
        {
            throw new NotImplementedException();
        }

        // Other methods and code remain unchanged
    }
}
