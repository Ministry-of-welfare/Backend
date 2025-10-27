using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BL.Api;
using BL.Models;
using Microsoft.AspNetCore.Mvc;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SystemsController : ControllerBase
    {
        private readonly IBlSystem _blSystem;

        public SystemsController(IBlSystem blSystem)
        {
            _blSystem = blSystem;
        }

        // GET: api/Systems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BlTSystem>>> GetSystems()
        {
            return Ok(await _blSystem.GetAll());
        }

        // GET: api/Systems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BlTSystem>> GetSystem(int id)
        {
            var system = await _blSystem.GetById(id);

            if (system == null)
            {
                return NotFound();
            }

            return Ok(system);
        }

        // POST: api/Systems
        [HttpPost]
        public async Task<ActionResult<BlTSystem>> PostSystem(BlTSystem system)
        {
            var createdSystem = await _blSystem.Create(system);

            return CreatedAtAction(nameof(GetSystem), new { id = createdSystem.SystemId }, createdSystem);
        }

        // PUT: api/Systems/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSystem(int id, BlTSystem system)
        {
            if (id != system.SystemId)
            {
                return BadRequest();
            }

            await _blSystem.Update(system);

            return NoContent();
        }

        // DELETE: api/Systems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSystem(int id)
        {
            await _blSystem.Delete(id);
            return NoContent();
        }

        [HttpGet("system-performance")]
        public async Task<ActionResult<IEnumerable<SystemPerformanceDto>>> GetSystemPerformanceAsync()
        {
            try
            {
                var performanceData = await _blSystem.GetSystemPerformanceAsync();

                if (performanceData == null || !performanceData.Any())
                {
                    return NotFound("No data found.");
                }

                return Ok(performanceData);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }


    }
}
