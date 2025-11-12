using Dal.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using System.Linq;

namespace server_pra.Controllers;

[ApiController]
[Route("[controller]")]
public class TabRolePermissionController : ControllerBase
{
    private readonly AppDbContext _context;

    public TabRolePermissionController(AppDbContext context)
    {
        _context = context;
    }

    // Create
    [HttpPost]
    public IActionResult Create([FromBody] TabRolePermission item)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        // אל תשלח RolePermissionId בפוסט - זה מפתח זהות (Identity)
        _context.TabRolePermissions.Add(item);
        _context.SaveChanges();

        return Ok(item);
    }

    // Read all
    [HttpGet]
    public IActionResult GetAll()
    {
        var list = _context.TabRolePermissions
            .Include(x => x.Role)
            .Include(x => x.Permission)
            .ToList();

        return Ok(list);
    }

    // Read one
    [HttpGet("{id}")]
    public IActionResult Get(int id)
    {
        var item = _context.TabRolePermissions
            .Include(x => x.Role)
            .Include(x => x.Permission)
            .FirstOrDefault(x => x.RolePermissionId == id);

        if (item == null)
            return NotFound();

        return Ok(item);
    }

    // Update
    [HttpPut("{id}")]
    public IActionResult Update(int id, [FromBody] TabRolePermission item)
    {
        if (id != item.RolePermissionId)
            return BadRequest("ID mismatch");

        if (!_context.TabRolePermissions.Any(x => x.RolePermissionId == id))
            return NotFound();

        _context.Entry(item).State = EntityState.Modified;
        _context.SaveChanges();

        return Ok(item);
    }

    // Delete
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var item = _context.TabRolePermissions.Find(id);
        if (item == null)
            return NotFound();

        _context.TabRolePermissions.Remove(item);
        _context.SaveChanges();

        return NoContent();
    }
}
