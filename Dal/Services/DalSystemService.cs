using System.Collections.Generic;
using System.Threading.Tasks;
using Dal.Api;
using Dal.Models;
using Microsoft.EntityFrameworkCore;
using SystemSystem = Dal.Models.System; // Alias to avoid conflict with System namespace

namespace Dal.Services
{
    public class DalSystemService : IDalSystem
    {
        private readonly AppDbContext _context;

        public DalSystemService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<SystemSystem>> GetAll() // Updated to use alias
        {
            return await _context.Systems.ToListAsync();
        }

        public async Task<SystemSystem> GetByIdAsync(int id) // Updated to use alias
        {
            return await _context.Systems.FindAsync(id);
        }

        public async Task Create(SystemSystem entity) // Updated to use alias
        {
            _context.Systems.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task Update(SystemSystem entity) // Updated to use alias
        {
            _context.Systems.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id) // No change needed here
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
