using Dal.Models;
using Microsoft.AspNetCore.Mvc;
using server_pra.Dal.Models.ScaffoldEntities;
using System.Linq;

namespace server_pra.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TabUserRoleController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TabUserRoleController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult Create(TabUserRole item)
        {
            _context.TabUserRoles.Add(item);
            _context.SaveChanges();
            return Ok(item);
        }

        [HttpGet]
        public IActionResult GetAll() => Ok(_context.TabUserRoles.ToList());

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var entity = _context.TabUserRoles.Find(id);
            if (entity == null) return NotFound();

            _context.TabUserRoles.Remove(entity);
            _context.SaveChanges();
            return Ok();
        }
    }
}
