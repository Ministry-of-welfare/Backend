using BL.Api;
using BL.Models;
using Dal.Api;

namespace BL.Services
{
    public class BlTemplateService : IBlTemplate
    {
        private readonly IDalTemplate _dal;

        public BlTemplateService(IDalTemplate dal)
        {
            _dal = dal;
        }

        public BlTemplate CastingTemplateFromBlToDal(BlTemplate? e)
        {
            throw new NotImplementedException();
        }

        public Task<BlTemplate> Create(BlTemplate item)
        {
            throw new NotImplementedException();
        }

        public Task Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<BlTemplate>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<BlTemplate> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<BlTemplate> Update(BlTemplate item)
        {
            throw new NotImplementedException();
        }
    }
}
