using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Dal.Models;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EnvironmentsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EnvironmentsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Environments/getAll
        [HttpGet("getAll")]

        public async Task<ActionResult<IEnumerable<Environment>>> GetEnvironments()
        {
            return await _context.Environments.ToListAsync();
        }

        // GET: api/Environments/get/5
        [HttpGet("get/{id}")]
        public async Task<ActionResult<Environment>> GetEnvironment(int id)
        {
            var environment = await _context.Environments.FindAsync(id);

            if (environment == null)
            {
                return NotFound();
            }

            return environment;
        }

        // POST: api/Environments/create
        [HttpPost("create")]
        public async Task<ActionResult<Environment>> PostEnvironment(Environment environment)
        {
            _context.Environments.Add(environment);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEnvironment), new { id = environment.EnvironmentId }, environment);
        }

        // PUT: api/Environments/5
        [HttpPut("update/{id}")]
        public async Task<IActionResult> PutEnvironment(int id, Environment environment)
        {
            if (id != environment.EnvironmentId)
            {
                return BadRequest();
            }

            _context.Entry(environment).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Environments/5
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteEnvironment(int id)
        {
            var environment = await _context.Environments.FindAsync(id);
            if (environment == null)
            {
                return NotFound();
            }

            _context.Environments.Remove(environment);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
