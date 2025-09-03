using Dal.Api;
using Dal.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dal.Services
{
    public class DalEnvironmentService : IdalEnvironment
    {
        private readonly AppDbContext _context;

        public DalEnvironmentService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Environment>> GetAll()
        {
            return await _context.Environments.ToListAsync();
        }
        

        public async Task<Environment?> GetByIdAsync(int id)
        {
            return await _context.Environments.FindAsync(id);
        }

        public async Task create(Environment entity)
        {
            _context.Environments.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task Update(Environment entity)
        {
            _context.Environments.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
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
