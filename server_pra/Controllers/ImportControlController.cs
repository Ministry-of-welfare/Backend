﻿using BL.Api;
using BL.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace server_pra.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ImportControlController : ControllerBase
    {


        private readonly IBlimportControl _blImportControl;

        public ImportControlController(IBlimportControl blImportControl)
        {
            _blImportControl = blImportControl;
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
    }
}
