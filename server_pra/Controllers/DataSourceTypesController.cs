using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BL.Api;
using BL.Models;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DataSourceTypesController : ControllerBase
    {
        private readonly IBlDataSourceType _blDataSourceType;

        public DataSourceTypesController(IBlDataSourceType blDataSourceType)
        {
            _blDataSourceType = blDataSourceType;
        }

        // GET: api/DataSourceTypes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BlDataSourceType>>> GetDataSourceTypes()
        {
            return Ok(await _blDataSourceType.GetAll());
        }

        // GET: api/DataSourceTypes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BlDataSourceType>> GetDataSourceType(int id)
        {
            var dataSourceType = await _blDataSourceType.GetById(id);

            if (dataSourceType == null)
            {
                return NotFound();
            }

            return Ok(dataSourceType);
        }

        // POST: api/DataSourceTypes
        [HttpPost]
        public async Task<ActionResult<BlDataSourceType>> PostDataSourceType(BlDataSourceType dataSourceType)
        {
            var createdDataSourceType = await _blDataSourceType.Create(dataSourceType);

            return CreatedAtAction(nameof(GetDataSourceType), new { id = createdDataSourceType.DataSourceTypeId }, createdDataSourceType);
        }

        // PUT: api/DataSourceTypes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDataSourceType(int id, BlDataSourceType dataSourceType)
        {
            if (id != dataSourceType.DataSourceTypeId)
            {
                return BadRequest();
            }

            await _blDataSourceType.Update(dataSourceType);

            return NoContent();
        }

        // DELETE: api/DataSourceTypes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDataSourceType(int id)
        {
            await _blDataSourceType.Delete(id);
            return NoContent();
        }
    }
}
