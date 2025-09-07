using System.Collections.Generic;
using System.Threading.Tasks;
using Dal.Api;
using Microsoft.EntityFrameworkCore;
using Dal.Models;

namespace Dal.Services
{
    public class DalSystemService : IDalSystem
    {
        private readonly AppDbContext _context;

        public DalSystemService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<System>> GetAll()
        {
            return await _context.Systems.ToListAsync();
        }

        public async Task<System> GetByIdAsync(int id)
        {
            return await _context.Systems.FindAsync(id);
        }

        public async Task Create(System entity)
        {
            _context.Systems.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task Update(System entity)
        {
            _context.Systems.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var entity = await _context.Systems.FindAsync(id);
            if (entity != null)
            {
                _context.Systems.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
