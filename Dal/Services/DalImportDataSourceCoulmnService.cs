using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dal.Api;
using Dal.Models;
using Microsoft.EntityFrameworkCore;

namespace Dal.Services
{
    internal class DalImportDataSourceColumnService : IDalImportDataSourceColumn
    {
        private readonly AppDbContext _db;

        private readonly string _connectionString;
        public DalImportDataSourceColumnService(AppDbContext db)
        {
            _db = db;
            _connectionString = _db.Database.GetConnectionString();
        }
        public async Task<TabImportDataSourceColumn> Create(TabImportDataSourceColumn item)
        {
            item.ImportDataSourceColumnsId = 0;
            _db.TabImportDataSourceColumns.Add(item);
            await _db.SaveChangesAsync();
            return item;
        }

        public async Task Delete(int id)
        {
            var entity = await _db.TabImportDataSourceColumns.FindAsync(id);
            if (entity != null)
            {
                _db.TabImportDataSourceColumns.Remove(entity);
                await _db.SaveChangesAsync();
            }
        }

        public async Task<List<TabImportDataSourceColumn>> GetAll()
        {
            return await _db.TabImportDataSourceColumns.ToListAsync();
        }

        public async Task<TabImportDataSourceColumn> GetById(int id)
        {
            return await _db.TabImportDataSourceColumns.FindAsync(id);
        }

        public async Task<TabImportDataSourceColumn> Update(TabImportDataSourceColumn item)
        {
            _db.TabImportDataSourceColumns.Update(item);
            await _db.SaveChangesAsync();
            return item;
        }
        Task ICrud<TabImportDataSourceColumn>.Update(TabImportDataSourceColumn item)
        {
            return Update(item);
        }

        Task ICrud<TabImportDataSourceColumn>.Create(TabImportDataSourceColumn item)
        {
            return Create(item);
        }

    }
}
