using System;
using System.Collections.Generic;
using System.Linq;
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
        //לדשבורד ביצועים לפי מערכת
        public async Task<IEnumerable<SystemPerformanceRawData>> GetSystemPerformanceDataAsync()
        {
            var result = await _context.Systems
                .GroupJoin(
                    _context.AppImportControls,
                    system => system.SystemId,
                    control => control.ImportDataSource.SystemId,
                    (system, controls) => new { system, controls }
                )
                .SelectMany(
                    x => x.controls.DefaultIfEmpty(),
                    (x, control) => new SystemPerformanceRawData
                    {
                        SystemId = x.system.SystemId,
                        SystemName = x.system.SystemName,
                        ImportControlId = control != null ? control.ImportControlId : 0,
                        ImportStatusId = control != null ? control.ImportStatusId : 0
                    }
                )
                .ToListAsync();

            return result;
        }

    }
}

