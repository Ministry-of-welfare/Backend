using Dal.Api;
using Dal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace Dal.Services
{
    public class DalImportControlService:IDalImportControl
    {

        private readonly AppDbContext _context;

        public DalImportControlService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<AppImportControl>> GetAll()
        {
            return await _context.AppImportControls.ToListAsync();
        }

        public async Task<AppImportControl> GetByIdAsync(int id)
        {
            return await _context.AppImportControls.FindAsync(id);
        }

        public async Task Create(AppImportControl entity)
        {
            _context.AppImportControls.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task Update(AppImportControl entity)
        {
            _context.AppImportControls.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var entity = await _context.AppImportControls.FindAsync(id);
            if (entity != null)
            {
                _context.AppImportControls.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }


    }
}
