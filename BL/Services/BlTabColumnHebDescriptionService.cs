using BL.Api;
using BL.Models;
using Dal.Api;

namespace BL.Services
{
    public class BlTabColumnHebDescriptionService : IBlTabColumnHebDescription
    {
        private readonly IDalTabColumnHebDescription _dal;

        public BlTabColumnHebDescriptionService(IDalTabColumnHebDescription dal)
        {
            _dal = dal;
        }

        public BlTabColumnHebDescription CastingTabColumnHebDescriptionFromBlToDal(BlTabColumnHebDescription? e)
        {
            throw new NotImplementedException();
        }

        public Task<BlTabColumnHebDescription> Create(BlTabColumnHebDescription item)
        {
            throw new NotImplementedException();
        }

        public Task Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<BlTabColumnHebDescription>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<BlTabColumnHebDescription> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<BlTabColumnHebDescription> Update(BlTabColumnHebDescription item)
        {
            throw new NotImplementedException();
        }
    }
}

