using BL.Api;
using BL.Models;
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

        public async Task<List<TabImportDataSource>> GetAll()
        {
            var data = await _dal.GetAll(); // Await the task to get the result
            return data.ToList(); // Use ToList on the result
        }

        //public async Task<TabImportDataSource> GetById(int id)
        //{
        //    return await Task.FromResult(_dal.GetById(id));
        //}

        public async Task<TabImportDataSource> Create(TabImportDataSource item)
        {
            _dal.Create(item);
            return await Task.FromResult(item);
        }

        public async Task<TabImportDataSource> Update(TabImportDataSource item)
        {
            _dal.Update(item);
            return await Task.FromResult(item);
        }

        public Task Delete(int id)
        {
            _dal.Delete(id);
            return Task.CompletedTask;
        }

        Task<List<BlTabImportDataSource>> IBlTabImportDataSource.GetAll()
        {
            throw new NotImplementedException();
        }

        Task<BlTabImportDataSource> IBlTabImportDataSource.GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<BlTabImportDataSource> Create(BlTabImportDataSource item)
        {
            throw new NotImplementedException();
        }

        public Task<BlTabImportDataSource> Update(BlTabImportDataSource item)
        {
            throw new NotImplementedException();
        }
    }
}
