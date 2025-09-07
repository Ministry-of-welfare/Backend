using BL.Api;
using Dal.Api;
using Dal.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BL.Services
{
    public class BlTabImportDataSourceService : IBlTabImportDataSource
    {
        private readonly IDalImportDataSource _dal;

        public BlTabImportDataSourceService(IDalImportDataSource dal)
        {
            _dal = dal;
        }

        public Task<List<TabImportDataSource>> GetAll()
        {
            return Task.FromResult(_dal.GetAll().ToList());
        }

        public Task<TabImportDataSource> GetById(int id)
        {
            return Task.FromResult(_dal.GetById(id));
        }

        public Task<TabImportDataSource> Create(TabImportDataSource item)
        {
            _dal.Create(item);
            return Task.FromResult(item);
        }

        public Task<TabImportDataSource> Update(TabImportDataSource item)
        {
            _dal.Update(item);
            return Task.FromResult(item);
        }

        public Task Delete(int id)
        {
            _dal.Delete(id);
            return Task.CompletedTask;
        }
    }
}
