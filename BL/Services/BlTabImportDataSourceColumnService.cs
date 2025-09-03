using BL.Api;
using BL.Models;
using Dal.Api;

namespace BL.Services
{
    public class BlTabImportDataSourceColumnService : IBlTabImportDataSourceColumn
    {
        private readonly IDalTabImportDataSourceColumn _dal;

        public BlTabImportDataSourceColumnService(IDalTabImportDataSourceColumn dal)
        {
            _dal = dal;
        }

        public BlTabImportDataSourceColumn CastingTabImportDataSourceColumnFromBlToDal(BlTabImportDataSourceColumn? e)
        {
            throw new NotImplementedException();
        }

        public Task<BlTabImportDataSourceColumn> Create(BlTabImportDataSourceColumn item)
        {
            throw new NotImplementedException();
        }

        public Task Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<BlTabImportDataSourceColumn>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<BlTabImportDataSourceColumn> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<BlTabImportDataSourceColumn> Update(BlTabImportDataSourceColumn item)
        {
            throw new NotImplementedException();
        }
    }
}

