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
    public class DalImportStatusService:IDalImportStatus
    {


        private readonly AppDbContext _context;

        public DalImportStatusService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<TImportStatus>> GetAll()
        {
            return await _context.TImportStatuses.ToListAsync();
        }

        public async Task<TImportStatus> GetByIdAsync(int id)
        {
            return await _context.TImportStatuses.FindAsync(id);
        }

        public async Task Create(TImportStatus entity)
        {
            _context.TImportStatuses.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task Update(TImportStatus entity)
        {
            _context.TImportStatuses.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var entity = await _context.TImportStatuses.FindAsync(id);
            if (entity != null)
            {
                _context.TImportStatuses.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        
    }
}
