using Dal.Models;
using Microsoft.AspNetCore.Mvc;

using System.Linq;

namespace server_pra.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TabRoleController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TabRoleController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult Create(TabRole item)
        {
            _context.TabRoles.Add(item);
            _context.SaveChanges();
            return Ok(item);
        }

        [HttpGet]
        public IActionResult GetAll() => Ok(_context.TabRoles.ToList());
    }
}
