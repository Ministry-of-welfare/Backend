//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using server_pra.Models; // לוודא שיש פה DbContext + TabImportDataSource
//using System.Collections.Generic;
//using System.Threading.Tasks;

//namespace server.Controllers
//{
//    [ApiController]
//    [Route("api/[controller]")]
//    public class ImportDataSourcesController : ControllerBase
//    {
//        private readonly AppDbContext _context;

//        public ImportDataSourcesController(AppDbContext context)
//        {
//            _context = context;
//        }

//        [HttpGet]
//        public async Task<ActionResult<IEnumerable<TabImportDataSource>>> GetAll()
//        {
//            return await _context.TabImportDataSources.ToListAsync();
//        }

//        [HttpGet("{id}")]
//        public async Task<ActionResult<TabImportDataSource>> GetById(int id)
//        {
//            var entity = await _context.TabImportDataSources.FindAsync(id);
//            if (entity == null)
//                return NotFound();
//            return entity;
//        }

//        [HttpPost]
//        public async Task<ActionResult<TabImportDataSource>> Create([FromBody] TabImportDataSource item)
//        {
//            _context.TabImportDataSources.Add(item);
//            await _context.SaveChangesAsync();

//            return CreatedAtAction(nameof(GetById), new { id = item.ImportDataSourceId }, item);
//        }

//        [HttpPut("{id}")]
//        public async Task<IActionResult> Update(int id, [FromBody] TabImportDataSource item)
//        {
//            if (id != item.ImportDataSourceId)
//                return BadRequest("ID mismatch");

//            _context.Entry(item).State = EntityState.Modified;

//            try
//            {
//                await _context.SaveChangesAsync();
//            }
//            catch (DbUpdateConcurrencyException)
//            {
//                if (!await _context.TabImportDataSources.AnyAsync(e => e.ImportDataSourceId == id))
//                    return NotFound();
//                throw;
//            }

//            return NoContent();
//        }

//        [HttpDelete("{id}")]
//        public async Task<IActionResult> Delete(int id)
//        {
//            var entity = await _context.TabImportDataSources.FindAsync(id);
//            if (entity == null)
//                return NotFound();

//            _context.TabImportDataSources.Remove(entity);
//            await _context.SaveChangesAsync();

//            return NoContent();
//        }
//    }
//}
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Dal.Models; // לוודא שזה אותו namespace של AppDbContext ושל ה־Entities
using System.Collections.Generic;
using System.Threading.Tasks;

namespace server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ImportDataSourcesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ImportDataSourcesController(AppDbContext context)
        {
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

        [HttpPost]
        public async Task<ActionResult<TabImportDataSource>> Create([FromBody] TabImportDataSource item)
        {
            _context.TabImportDataSources.Add(item);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = item.ImportDataSourceId }, item);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] TabImportDataSource item)
        {
            if (id != item.ImportDataSourceId)
                return BadRequest("ID mismatch");

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
    }
}
