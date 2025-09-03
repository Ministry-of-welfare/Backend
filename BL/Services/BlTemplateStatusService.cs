using BL.Api;
using BL.Models;
using Dal.Api;

namespace BL.Services
{
    public class BlTemplateStatusService : IBlTemplateStatus
    {
        private readonly IDalTemplateStatus _dal;

        public BlTemplateStatusService(IDalTemplateStatus dal)
        {
            _dal = dal;
        }

        public BlTemplateStatus CastingTemplateStatusFromBlToDal(BlTemplateStatus? e)
        {
            throw new NotImplementedException();
        }

        public Task<BlTemplateStatus> Create(BlTemplateStatus item)
        {
            throw new NotImplementedException();
        }

        public Task Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<BlTemplateStatus>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<BlTemplateStatus> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<BlTemplateStatus> Update(BlTemplateStatus item)
        {
            throw new NotImplementedException();
        }
    }
}
