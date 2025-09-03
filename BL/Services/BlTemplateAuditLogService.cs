using BL.Api;
using BL.Models;
using Dal.Api;

namespace BL.Services
{
    public class BlTemplateAuditLogService : IBlTemplateAuditLog
    {
        private readonly IDalTemplateAuditLog _dal;

        public BlTemplateAuditLogService(IDalTemplateAuditLog dal)
        {
            _dal = dal;
        }

        public BlTemplateAuditLog CastingTemplateAuditLogFromBlToDal(BlTemplateAuditLog? e)
        {
            throw new NotImplementedException();
        }

        public Task<BlTemplateAuditLog> Create(BlTemplateAuditLog item)
        {
            throw new NotImplementedException();
        }

        public Task Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<BlTemplateAuditLog>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<BlTemplateAuditLog> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<BlTemplateAuditLog> Update(BlTemplateAuditLog item)
        {
            throw new NotImplementedException();
        }
    }
}
