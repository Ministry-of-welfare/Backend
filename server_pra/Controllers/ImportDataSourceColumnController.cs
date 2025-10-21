using BL.Api;
using Dal.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace server_pra.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ImportDataSourceColumnController : ControllerBase // Change from base class to ControllerBase to use helper methods.
    {
        private readonly AppDbContext _context;
        private readonly IBl _bl;

        public ImportDataSourceColumnController(AppDbContext context, IBl bl)
        {
            _bl = bl;
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TabImportDataSourceColumn>>> GetAll()
        {
            return await _context.TabImportDataSourceColumns.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TabImportDataSourceColumn>> GetById(int id)
        {
            var entity = await _context.TabImportDataSourceColumns.FindAsync(id);
            if (entity == null)
                return NotFound();
            return entity;
        }

        [HttpPost]
        public async Task<ActionResult<TabImportDataSourceColumn>> Create([FromBody] TabImportDataSourceColumn item)
        {
            var blItem = BL.Services.BlTabImportDataSourceColumnService.ToBl(item);
            await _bl.TabImportDataSourceColumn.Create(blItem);
            return CreatedAtAction(nameof(GetById), new { id = item.ImportDataSourceColumnsId }, item);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _context.TabImportDataSourceColumns.FindAsync(id);
            if (entity == null)
                return NotFound();

            _context.TabImportDataSourceColumns.Remove(entity);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
