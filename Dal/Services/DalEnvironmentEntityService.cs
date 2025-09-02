using Dal.Api;
using Dal.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dal.Services
{
    public class DalEnvironmentEntityService : IEnvironmentEntity
    {
        private readonly AppDbContext _context;

        public DalEnvironmentEntityService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<EnvironmentEntity>> GetAllAsync()
        {
            return await _context.Environments.ToListAsync();
        }

        public async Task<EnvironmentEntity?> GetByIdAsync(int id)
        {
            return await _context.Environments.FindAsync(id);
        }

        public async Task AddAsync(EnvironmentEntity entity)
        {
            _context.Environments.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(EnvironmentEntity entity)
        {
            _context.Environments.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.Environments.FindAsync(id);
            if (entity != null)
            {
                _context.Environments.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
