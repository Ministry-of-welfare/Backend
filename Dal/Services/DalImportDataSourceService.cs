using Dal.Api;
using Dal.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dal.Services
{
    public class DalImportDataSourceService : IDalImportDataSource
    {
        private readonly AppDbContext _db;

        public DalImportDataSourceService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<List<TabImportDataSource>> GetAll()
        {
            return await _db.TabImportDataSources.ToListAsync();
        }

        public async Task<TabImportDataSource?> GetById(int id)
        {
            return await _db.TabImportDataSources.FindAsync(id);
        }

        public async Task<TabImportDataSource> Create(TabImportDataSource item)
        {
            _db.TabImportDataSources.Add(item);
            await _db.SaveChangesAsync();
            return item;
        }

        public async Task<TabImportDataSource> Update(TabImportDataSource item)
        {
            _db.TabImportDataSources.Update(item);
            await _db.SaveChangesAsync();
            return item;
        }

        public async Task Delete(int id)
        {
            var entity = await _db.TabImportDataSources.FindAsync(id);
            if (entity != null)
            {
                _db.TabImportDataSources.Remove(entity);
                await _db.SaveChangesAsync();
            }
        }

       

        Task ICrud<TabImportDataSource>.Update(TabImportDataSource item)
        {
            return Update(item);
        }

        Task ICrud<TabImportDataSource>.Create(TabImportDataSource item)
        {
            return Create(item);
        }
    }
}
