using Microsoft.AspNetCore.Mvc;
using BL.Api;
using BL.Models;

namespace server_pra.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EnvironmentsController : ControllerBase
    {
        private readonly IblEnvironmentEntityService _service;

        public EnvironmentsController(IblEnvironmentEntityService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BlEnvironmentEntity>>> GetAll()
        {
            var environments = await _service.GetAllAsync();
            return Ok(environments);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BlEnvironmentEntity>> GetById(int id)
        {
            var env = await _service.GetByIdAsync(id);
            if (env == null)
                return NotFound();

            return Ok(env);
        }

        [HttpPost]
        public async Task<ActionResult> Create(BlEnvironmentEntity env)
        {
            await _service.AddAsync(env);
            return CreatedAtAction(nameof(GetById), new { id = env.EnvironmentId }, env);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, BlEnvironmentEntity env)
        {
            if (id != env.EnvironmentId)
                return BadRequest();

            var updated = await _service.UpdateAsync(env);
            if (!updated)
                return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteAsync(id);
            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}
