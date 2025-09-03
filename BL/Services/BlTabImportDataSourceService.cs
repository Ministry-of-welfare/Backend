using BL.Api;
using BL.Models;
using Dal.Api;

namespace BL.Services
{
    public class BlTabImportDataSourceService : IBlTabImportDataSource
    {
        private readonly IDalTabImportDataSource _dal;

        public BlTabImportDataSourceService(IDalTabImportDataSource dal)
        {
            _dal = dal;
        }

        public BlTabImportDataSource CastingTabImportDataSourceFromBlToDal(BlTabImportDataSource? e)
        {
            throw new NotImplementedException();
        }

        public Task<BlTabImportDataSource> Create(BlTabImportDataSource item)
        {
            throw new NotImplementedException();
        }

        public Task Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<BlTabImportDataSource>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<BlTabImportDataSource> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<BlTabImportDataSource> Update(BlTabImportDataSource item)
        {
            throw new NotImplementedException();
        }
    }
}

