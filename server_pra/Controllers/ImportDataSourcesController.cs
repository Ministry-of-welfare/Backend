

using BL.Api;
using BL.Models;
using BL.Services;
using Dal.Models; // ����� ��� ���� namespace �� AppDbContext ��� ��Entities
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using server_pra.Models;
using Serilog;
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

        public ImportDataSourcesController(AppDbContext context, IBl bl)
        {
            _bl = bl;
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TabImportDataSource>>> GetAll()
        {
            try
            {
                Log.Information("Getting all ImportDataSources");
                var result = await _context.TabImportDataSources.ToListAsync();
                Log.Information("Retrieved {Count} ImportDataSources", result.Count);
                return result;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error getting ImportDataSources");
                throw;
            }
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
            var updated = await _bl.TabImportDataSource.UpdateEndDate(id); // BL ����� BL model
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
            // ����� ����� ���� ����� �����
            if (item.StartDate != null && item.EndDate != null && item.EndDate < item.StartDate)
            {
                return BadRequest(new { message = "����� ����� �� ���� ����� ���� ����� ������." });
            }

            // ����� ������ ������ ����
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
            await _bl.TabImportDataSource.Create(item);          // ��� ����� ���
            return Ok(new { message = "���� ������" });       // ���� ����� ����� ��� �����
        }

        //����� ������ id

        [HttpPost("CreateAndReturnId")]
        public async Task<int> CreateAndReturnId([FromBody] BlTabImportDataSource item)
        {
            // ����� ���� ������
            if (item.FileStatusId == null || item.FileStatusId == 0)
            {
                item.FileStatusId = 3; // �����
            }

            var result = await _bl.TabImportDataSource.CreateAndReturnId(item);
            return result;
        }


        //����� ���� �������
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


        //// �������� ����� ���� ������ ������ 
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
