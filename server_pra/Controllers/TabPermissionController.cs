using Dal.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using System.Linq;


[ApiController]
[Route("[controller]")]
public class TabPermissionController : ControllerBase
{
    private readonly AppDbContext _context;

    public TabPermissionController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public IActionResult Create([FromBody] TabPermission item)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        _context.TabPermissions.Add(item);
        _context.SaveChanges();

        return Ok(item);
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var data = _context.TabPermissions.ToList();
        return Ok(data);
    }

    [HttpGet("{id}")]
    public IActionResult Get(int id)
    {
        var item = _context.TabPermissions.Find(id);
        if (item == null)
            return NotFound();

        return Ok(item);
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, [FromBody] TabPermission item)
    {
        if (id != item.PermissionId)
            return BadRequest();

        if (!_context.TabPermissions.Any(x => x.PermissionId == id))
            return NotFound();

        _context.Entry(item).State = EntityState.Modified;
        _context.SaveChanges();

        return Ok(item);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var item = _context.TabPermissions.Find(id);
        if (item == null)
            return NotFound();

        _context.TabPermissions.Remove(item);
        _context.SaveChanges();

        return NoContent();
    }
}
