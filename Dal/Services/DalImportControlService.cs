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
        public async Task UpdateImportStatusAsync(int importId, int rowsInvalid, int totalRowsAffected, string status)
        {
            var entity = await _context.AppImportControls.FindAsync(importId);
            if (entity == null)
                throw new Exception($"AppImportControl not found with ID {importId}");

            Console.WriteLine($"Attempting to update import status for Import ID: {importId}, Status: {status}");

            var statusEntity = await _context.TImportStatuses
                .FirstOrDefaultAsync(s => s.ImportStatusDesc == status);

            if (statusEntity == null)
            {
                Console.WriteLine($"Warning: Status '{status}' not found in TImportStatuses. Skipping status update.");
            }
            else
            {
                Console.WriteLine($"Found status '{status}' with ID: {statusEntity.ImportStatusId}");
                entity.ImportStatusId = statusEntity.ImportStatusId;
            }

            entity.RowsInvalid = rowsInvalid;
            entity.TotalRowsAffected = totalRowsAffected;

            await _context.SaveChangesAsync();
        }
        public async Task<int> CountImportProblems(int importControlId)
        {
            return await _context.AppImportProblems
                .CountAsync(p => p.ImportControlId == importControlId);
        }
        public async Task UpdateErrorReportPathAsync(int importControlId, string filePath)
        {
            var entity = await _context.AppImportControls.FindAsync(importControlId);
            if (entity == null)
                throw new Exception($"AppImportControl not found with ID {importControlId}");

            entity.ErrorReportPath = filePath;
            await _context.SaveChangesAsync();
        }
        public async Task<TabImportDataSource> GetImportDataSourceByIdAsync(int importDataSourceId)
        {
            return await _context.TabImportDataSources.FindAsync(importDataSourceId);
        }
        public async Task<Dictionary<string, string>> GetColumnDescriptionsAsync(int importDataSourceId)
        {
            return await _context.TabColumnHebDescriptions
                .Where(c => c.ImportDataSourceId == importDataSourceId)
                .ToDictionaryAsync(c => c.ColumnName, c => c.ColumnDescription);
        }

    }
}
