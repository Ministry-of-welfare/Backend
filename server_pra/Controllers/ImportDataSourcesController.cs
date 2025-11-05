

using BL.Api;
using BL.Models;
using BL.Services;
using Dal.Models; // ����� ��� ���� namespace �� AppDbContext ��� ��Entities
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using server_pra.Models;
using server_pra.Services;
using Serilog;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
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
        private readonly Microsoft.Extensions.Logging.ILogger<ImportDataSourcesController> _logger;

        public ImportDataSourcesController(AppDbContext context, IBl bl, Microsoft.Extensions.Logging.ILogger<ImportDataSourcesController> logger)
        {
            _bl = bl;
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TabImportDataSource>>> GetAll()
        {
            try
            {
                var result = await _context.TabImportDataSources.ToListAsync();
                var userName = GetUserName();
                Log.ForContext("UserName", userName)
                   .Information("Retrieved {Count} ImportDataSources", result.Count);
                return result;
            }
            catch (Exception ex)
            {
                Log.ForContext("UserName", GetUserName())
                   .Error(ex, "Error getting ImportDataSources");
                throw;
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TabImportDataSource>> GetById(int id)
        {
            try
            {
                var entity = await _context.TabImportDataSources.FindAsync(id);
                var userName = GetUserName();
                if (entity == null)
                {
                    Log.ForContext("UserName", userName)
                       .Warning("ImportDataSource not found with ID: {Id}", id);
                    return NotFound();
                }
                Log.ForContext("UserName", userName)
                   .Information("Retrieved ImportDataSource with ID: {Id}", id);
                return entity;
            }
            catch (Exception ex)
            {
                Log.ForContext("UserName", GetUserName())
                   .Error(ex, "Error getting ImportDataSource by ID: {Id}", id);
                throw;
            }
        }


       

        [HttpPut("updateJustEndDate/{id}")]
        public async Task<IActionResult> UpdateEndDate(int id)
        {
            var updated = await _bl.TabImportDataSource.UpdateEndDate(id); 
            if (updated == null)
                return NotFound();

            return Ok(new { message = "����� ������" });
        }
        [HttpPut("update{id}")]
        public async Task<IActionResult> Update(int id, BlTabImportDataSource ds)
        {
            if (id != ds.ImportDataSourceId)
            {
                return BadRequest(new { message = "ID mismatch between URL and body." });
            }

            if (ds.StartDate != null && ds.EndDate != null)
            {
                if (ds.EndDate < ds.StartDate)
                {
                    return BadRequest(new { message = "����� ����� �� ���� ����� ���� ����� ������." });
                }
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
           
            if (item.StartDate != null && item.EndDate != null && item.EndDate < item.StartDate)
            {
                return BadRequest(new { message = "����� ����� �� ���� ����� ���� ����� ������." });
            }

            if (!string.IsNullOrWhiteSpace(item.ErrorRecipients))
            {
                var emailPattern = @"^[A-Za-z0-9\u0590-\u05FF._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$";
                var recipients = item.ErrorRecipients
                    .Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);

                foreach (var email in recipients)
                {
                    var trimmed = email.Trim();
                    if (!Regex.IsMatch(trimmed, emailPattern))
                    {
                        return BadRequest(new { message = $"����� ����� '{trimmed}' ���� �����." });
                    }
                }
            }
            await _bl.TabImportDataSource.Create(item);         
            return Ok(new { message = "���� ������" });      
        }

    

        [HttpPost("CreateAndReturnId")]
        public async Task<int> CreateAndReturnId([FromBody] BlTabImportDataSource item)
        {
         
            if (item.FileStatusId == null || item.FileStatusId == 0)
            {
                item.FileStatusId = 3;
            }

            var result = await _bl.TabImportDataSource.CreateAndReturnId(item);
            return result;
        }


        
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
                return Ok($"���� ����� ������ ���� ImportDataSourceId {id}");
            }
            catch (Exception ex)
            {
                return BadRequest($"�����: {ex.Message}\n{ex.StackTrace}");
            }
        }

        [HttpPut("update-status/{id}")]
        public async Task<IActionResult> UpdateStatusOnly(int id, [FromBody] int newStatusId)

        {
            var item = await _context.TabImportDataSources.FindAsync(id);
            if (item == null)
                return NotFound();

            item.FileStatusId = newStatusId;

            try
            {
                await _context.SaveChangesAsync();
                return Ok(new { message = "����� ����� ������" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "����� ��� ����� ������", details = ex.Message });
            }
        }


     
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

        [HttpPost("load-bulk-data")]
        public async Task<IActionResult> LoadBulkData([FromBody] LoadBulkDataRequest request)
        {
            try
            {
                var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
                var bulkLogger = loggerFactory.CreateLogger<LoadBulkTable>();
                var loadBulkService = new LoadBulkTable(_context, bulkLogger);
                await loadBulkService.LoadBulkData(request.ImportDataSourceId, request.ImportControlId);
                return Ok(new { message = "Data loaded successfully", importDataSourceId = request.ImportDataSourceId });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error loading data", error = ex.Message });
            }
        }

        private string GetUserName()
        {
            // נסה לקחת מ-Authentication
            if (!string.IsNullOrEmpty(User?.Identity?.Name))
                return User.Identity.Name;
            
            // נסה לקחת מ-Headers
            if (Request.Headers.ContainsKey("X-User-Name"))
                return Request.Headers["X-User-Name"].ToString();
            
            if (Request.Headers.ContainsKey("User-Name"))
                return Request.Headers["User-Name"].ToString();
            
            // קח IP Address
            var ipAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            if (!string.IsNullOrEmpty(ipAddress))
                return $"IP:{ipAddress}";
            
            return "Anonymous";
        }

        public class LoadBulkDataRequest
        {
            public int ImportDataSourceId { get; set; }
            public int ImportControlId { get; set; }
        }
    }
}
