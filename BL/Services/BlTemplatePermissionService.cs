using BL.Api;
using BL.Models;
using Dal.Api;

namespace BL.Services
{
    public class BlTemplatePermissionService : IBlTemplatePermission
    {
        private readonly IDalTemplatePermission _dal;

        public BlTemplatePermissionService(IDalTemplatePermission dal)
        {
            _dal = dal;
        }

        public BlTemplatePermission CastingTemplatePermissionFromBlToDal(BlTemplatePermission? e)
        {
            throw new NotImplementedException();
        }

        public Task<BlTemplatePermission> Create(BlTemplatePermission item)
        {
            throw new NotImplementedException();
        }

        public Task Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<BlTemplatePermission>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<BlTemplatePermission> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<BlTemplatePermission> Update(BlTemplatePermission item)
        {
            throw new NotImplementedException();
        }
    }
}
