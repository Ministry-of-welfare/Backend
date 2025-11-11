using Dal.Models;
using Microsoft.AspNetCore.Mvc;
using server_pra.Dal.Models.ScaffoldEntities;
using System.Linq;

namespace server_pra.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TabUserController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TabUserController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult Create(TabUser item)
        {
            _context.TabUsers.Add(item);
            _context.SaveChanges();
            return Ok(item);
        }

        [HttpGet]
        public IActionResult GetAll() => Ok(_context.TabUsers.ToList());

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var entity = _context.TabUsers.Find(id);
            return entity == null ? NotFound() : Ok(entity);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, TabUser item)
        {
            var entity = _context.TabUsers.Find(id);
            if (entity == null) return NotFound();

            entity.UserName = item.UserName;
            entity.Email = item.Email;
            entity.FirstName = item.FirstName;
            entity.LastName = item.LastName;
            entity.IsActive = item.IsActive;
            entity.DisplayName = item.DisplayName;

            _context.SaveChanges();
            return Ok(entity);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var entity = _context.TabUsers.Find(id);
            if (entity == null) return NotFound();

            _context.TabUsers.Remove(entity);
            _context.SaveChanges();
            return Ok();
        }
    }
}
