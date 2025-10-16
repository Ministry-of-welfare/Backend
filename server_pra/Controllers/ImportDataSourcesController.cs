

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BL.Api;
using BL.Models;
using BL.Services;
using Dal.Models;
using Dal.Models; // לוודא שזה אותו namespace של AppDbContext ושל ה־Entities
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using server_pra.Models;
using AppDbContext = Dal.Models.AppDbContext;
using TabImportDataSource = Dal.Models.TabImportDataSource;


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


       

        [HttpPut("updateJustEndDate/{id}")]
        public async Task<IActionResult> UpdateEndDate(int id)
        {
            var updated = await _bl.TabImportDataSource.UpdateEndDate(id); // BL מחזיר BL model
            if (updated == null)
                return NotFound();

            return Ok(new { message = "עודכן בהצלחה" });
        }
        [HttpPut("update{id}")]
        public async Task<IActionResult> Update(int id, BlTabImportDataSource ds)
        {
            if (id != ds.DataSourceTypeId)
            {
                return BadRequest();
            }

            await _bl.TabImportDataSource.Update(ds);

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

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] BlTabImportDataSource item)
        {
            await _bl.TabImportDataSource.Create(item);          // אין החזרת ערך
            return Ok(new { message = "נוצר בהצלחה" });       // ניתן לשנות הודעה לפי הצורך
        }

        //הוספה והחזרת id
        [HttpPost("CreateAndReturnId")]
        public async Task<IActionResult> CreateAndReturnId([FromBody] BlTabImportDataSource item)
        {
            await _bl.TabImportDataSource.Create(item);          // אין החזרת ערך
            return Ok(new { message = "נוצר בהצלחה" });       // ניתן לשנות הודעה לפי הצורך
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
        //// פונקציית חיפוש למסך קליטות שבוצעו 
        [HttpGet("search")]
        public async Task<IActionResult> SearchImportDataSources(
            [FromQuery] DateTime? startDate,
            [FromQuery] DateTime? endDate,
            [FromQuery] int? systemId,
            [FromQuery] string systemName,
            [FromQuery] string importDataSourceDesc,
            [FromQuery] int? importStatusId,
            [FromQuery] string fileName,
            [FromQuery] bool showErrorsOnly)
        {
            var results = await _bl.TabImportDataSource.SearchImportDataSourcesAsync(
                startDate, endDate, systemId, systemName, importDataSourceDesc, importStatusId, fileName, showErrorsOnly);

            return Ok(results);
        }



    }
}
