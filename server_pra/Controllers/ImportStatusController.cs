using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BL.Api;
using BL.Models;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ImportStatusController : ControllerBase
    {
        private readonly IBlImportStatus _ImportStatus;

        public ImportStatusController(IBlImportStatus blImpertStatus)
        {
            _ImportStatus = blImpertStatus;
        }

        // GET: api/Systems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BlTImportStatus>>> GetSystems()
        {
            return Ok(await _ImportStatus.GetAll());
        }

        // GET: api/Systems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BlTImportStatus>> GetSystem(int id)
        {
            var status = await _ImportStatus.GetById(id);

            if (status == null)
            {
                return NotFound();
            }

            return Ok(status);
        }

        // POST: api/Systems
        [HttpPost]
        public async Task<ActionResult<BlTImportStatus>> PostSystem(BlTImportStatus status)
        {
            var createdStatus = await _ImportStatus.Create(status);

            return CreatedAtAction(nameof(GetSystem), new { id = createdStatus.ImportStatusId }, createdStatus);
        }

        // PUT: api/Systems/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStatus(int id, BlTImportStatus status)
        {
            if (id != status.ImportStatusId)
            {
                return BadRequest();
            }

            await _ImportStatus.Update(status);

            return NoContent();
        }

        // DELETE: api/Systems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStatus(int id)
        {
            await _ImportStatus.Delete(id);
            return NoContent();
        }

        
    }
}
