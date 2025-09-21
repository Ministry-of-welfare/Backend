using Dal.Api;
using Dal.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal.Services
{
    public class DalFileStatusService : IDalFileStatus
    {

        private readonly AppDbContext _context;

        public DalFileStatusService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<TFileStatus>> GetAll()
        {
            return await _context.TFileStatuses.ToListAsync();
        }

        public async Task<TFileStatus> GetByIdAsync(int id)
        {
            return await _context.TFileStatuses.FindAsync(id);
        }

        public async Task Create(TFileStatus entity)
        {
            _context.TFileStatuses.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task Update(TFileStatus entity)
        {
            _context.TFileStatuses.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var entity = await _context.TFileStatuses.FindAsync(id);
            if (entity != null)
            {
                _context.TFileStatuses.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }


    }
}


