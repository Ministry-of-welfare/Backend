using System.Collections.Generic;
using System.Threading.Tasks;
using Dal.Api;
using Microsoft.EntityFrameworkCore;
using Dal.Models;

namespace Dal.Services
{
    public class DalDataSourceTypeService : IDalDataSourceType
    {
        private readonly AppDbContext _context;

        public DalDataSourceTypeService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<TDataSourceType>> GetAll()
        {
            return await _context.TDataSourceTypes.ToListAsync(); // Updated property name
        }

        public async Task<TDataSourceType> GetByIdAsync(int id)
        {
            return await _context.TDataSourceTypes.FindAsync(id); // Updated property name
        }

        public async Task Create(TDataSourceType entity)
        {
            _context.TDataSourceTypes.Add(entity); // Updated property name
            await _context.SaveChangesAsync();
        }

        public async Task Update(TDataSourceType entity)
        {
            _context.TDataSourceTypes.Update(entity); // Updated property name
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var entity = await _context.TDataSourceTypes.FindAsync(id); // Updated property name
            if (entity != null)
            {
                _context.TDataSourceTypes.Remove(entity); // Updated property name
                await _context.SaveChangesAsync();
            }
        }
    }
}
