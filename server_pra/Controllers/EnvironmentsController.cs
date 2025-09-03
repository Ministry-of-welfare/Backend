using System.Collections.Generic;
using System.Threading.Tasks;
using Dal.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        // GET: api/Environments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DalEnvironment>>> GetEnvironments()
        {
            return await _context.Environments.ToListAsync();
        }

        // GET: api/Environments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DalEnvironment>> GetEnvironment(int id)
        {
            var environment = await _context.Environments.FindAsync(id);

            if (environment == null)
            {
                return NotFound();
            }

            return environment;
        }

        // POST: api/Environments
        [HttpPost]
        public async Task<ActionResult<DalEnvironment>> PostEnvironment(DalEnvironment environment)
        {
            _context.Environments.Add(environment);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEnvironment), new { id = environment.EnvironmentId }, environment);
        }

        // PUT: api/Environments/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEnvironment(int id, DalEnvironment environment)
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
        [HttpDelete("{id}")]
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
