using Dal.Models;
using Dal.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class FileStatusController : ControllerBase
{
    private readonly DalFileStatusService _dalFileStatusService;

    public FileStatusController(DalFileStatusService dalFileStatusService)
    {
        _dalFileStatusService = dalFileStatusService;
    }

    [HttpGet]
    public async Task<ActionResult<List<TFileStatus>>> GetAll()
    {
        return await _dalFileStatusService.GetAll();
    }

    //[HttpGet("{id}")]
    //public async Task<ActionResult<TFileStatus>> GetById(int id)
    //{
    //    var result = await _dalFileStatusService.GetById(id);
    //    if (result == null) return NotFound();
    //    return result;
    //}

    [HttpPost]
    public async Task<IActionResult> Create(TFileStatus entity)
    {
        await _dalFileStatusService.Create(entity);
        return Ok();
    }

    [HttpPut]
    public async Task<IActionResult> Update(TFileStatus entity)
    {
        await _dalFileStatusService.Update(entity);
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _dalFileStatusService.Delete(id);
        return Ok();
    }
}