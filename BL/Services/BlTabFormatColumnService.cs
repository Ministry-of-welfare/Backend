using BL.Api;
using BL.Models;
using Dal.Api;

namespace BL.Services
{
    public class BlTabFormatColumnService : IBlTabFormatColumn
    {
        private readonly IDalTabFormatColumn _dal;

        public BlTabFormatColumnService(IDalTabFormatColumn dal)
        {
            _dal = dal;
        }

        public BlTabFormatColumn CastingTabFormatColumnFromBlToDal(BlTabFormatColumn? e)
        {
            throw new NotImplementedException();
        }

        public Task<BlTabFormatColumn> Create(BlTabFormatColumn item)
        {
            throw new NotImplementedException();
        }

        public Task Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<BlTabFormatColumn>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<BlTabFormatColumn> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<BlTabFormatColumn> Update(BlTabFormatColumn item)
        {
            throw new NotImplementedException();
        }
    }
}

