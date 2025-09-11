

using BL.Api;
using Dal.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dal.Models; // לוודא שזה אותו namespace של AppDbContext ושל ה־Entities


namespace server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ImportDataSourcesController : ControllerBase
    {
        private readonly AppDbContext _context;

        private readonly IBl _bl;

        public ImportDataSourcesController(AppDbContext context, IBl bl)
        {
            _bl = bl;
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TabImportDataSource>>> GetAll()
        {
            return await _context.TabImportDataSources.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TabImportDataSource>> GetById(int id)
        {
            var entity = await _context.TabImportDataSources.FindAsync(id);
            if (entity == null)
                return NotFound();
            return entity;
        }


        private void SetEndDateToNow(TabImportDataSource item)
        {
            item.EndDate = DateTime.Now;
        }

        [HttpPut("updateJustEndDate/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] TabImportDataSource item)
        {
            if (id != item.ImportDataSourceId)
                return BadRequest("ID mismatch");

            // קריאה לפונקציה שמעדכנת את EndDate
            SetEndDateToNow(item);

            _context.Entry(item).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.TabImportDataSources.AnyAsync(e => e.ImportDataSourceId == id))
                    return NotFound();
                throw;
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _context.TabImportDataSources.FindAsync(id);
            if (entity == null)
                return NotFound();

            _context.TabImportDataSources.Remove(entity);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<TabImportDataSource>> Create([FromBody] TabImportDataSource item)
        {
            _context.TabImportDataSources.Add(item);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = item.ImportDataSourceId }, item);
        }
        //יצירת טבלה דינאמית
        [HttpPost("{id}/create-table")]
        public IActionResult CreateDynamicTable(int id)
        {
            try
            {
                if (_bl == null)
                    throw new Exception("_bl is null");
                if (_bl.TabImportDataSource == null)
                    throw new Exception("_bl.TabImportDataSource is");


                                _bl.TabImportDataSource.CreateDynamicTable(id);
                return Ok($"טבלה נוצרה בהצלחה עבור ImportDataSourceId {id}");
            }
            catch (Exception ex)
            {
                return BadRequest($"שגיאה: {ex.Message}\n{ex.StackTrace}");
            }
        }

        

    }
}
