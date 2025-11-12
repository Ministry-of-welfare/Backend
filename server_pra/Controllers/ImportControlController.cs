using BL.Api;
using BL.Models;
using Microsoft.AspNetCore.Mvc;
using server_pra.Services;
using System;
using server_pra.Services; // Add this to import ErrorReportService
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace server_pra.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ImportControlController : ControllerBase
    {
        private readonly IBlimportControl _blImportControl;
        private readonly ValidationService _validationService;

        public ImportControlController(IBlimportControl blImportControl, ValidationService validationService)
        {
            _blImportControl = blImportControl;
            _validationService = validationService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BlAppImportControl>>> GetImportControls()
        {
            return Ok(await _blImportControl.GetAll());
        }

        // GET: api/ImportControl/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BlAppImportControl>> GetImportcontrol(int id)
        {
            var importontrol = await _blImportControl.GetById(id);

            if (importontrol == null)
            {
                return NotFound();
            }

            return Ok(importontrol);
        }

        [HttpPost]
        public async Task<ActionResult<BlAppImportControl>> PostImportControl(BlAppImportControl importControl)
        {
            var createdImportControl = await _blImportControl.Create(importControl);

            return CreatedAtAction(nameof(GetImportcontrol), new { id = createdImportControl.ImportControlId }, createdImportControl);
        }

        //PUT: api/ImportControl/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutImportControl(int id, BlAppImportControl importControl)
        {
            if (id != importControl.ImportControlId)
            {
                return BadRequest();
            }

            await _blImportControl.Update(importControl);

            return NoContent();
        }

        //DELETE: api/ImportControl/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteImportControl(int id)
        {
            await _blImportControl.Delete(id);
            return NoContent();
        }

        [HttpPost("{importControlId}/validate")]
        public async Task<IActionResult> ValidateImportControl(int importControlId)
        {
            try
            {
                await _validationService.ValidateAsync(importControlId);
                return Ok(new { message = "Validation completed successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Validation failed.", details = ex.Message });
            }
        }


        
        [HttpPost("{id}/generate-error-report")]
        public async Task<IActionResult> GenerateErrorReport(int id, [FromServices] ErrorReportService errorReportService)
        {
            await errorReportService.GenerateAndSendErrorReportAsync(id);
            return Ok($"Error report generated and sent for ImportControlId {id}");

        }

    }
    }
