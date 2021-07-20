using Microsoft.AspNetCore.Mvc;
using RecrutariBlazorWasm.Server.Extensions;
using RecrutariBlazorWasm.Server.Interfaces;
using RecrutariBlazorWasm.Shared.Entities;
using RecrutariBlazorWasm.Shared.Helpers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RecrutariBlazorWasm.Server.Controllers
{
    public class ProjectController : BaseApiController
    {
        private readonly IProjectRepository _context;

        public ProjectController(IProjectRepository context)
        {
            this._context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Project>>> Get([FromQuery] PaginationParams paginationParams)
        {
            var result = await _context.GetProjects(paginationParams);
            Response.AddPaginationHeader(paginationParams.PageNumber, paginationParams.PageSize, result.TotalItems);

            if (result != null)
            {
                return Ok(result.Items);
            }
            else
            {
                return NotFound("Could not get projects!");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Project>> Get(int id)
        {
            var result = await _context.GetProjectById(id);

            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                return NotFound("Could not find requested project!");
            }
        }

        [HttpPost]
        public async Task<ActionResult<int>> Post(Project project)
        {
            var result = await _context.CreateProject(project);
            if (result > 0)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest("Could not create project!");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<int>> Put(int id, [FromBody] Project project)
        {
            var result = await _context.UpdateProject(id, project);
            if (result > 0)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest("Could not edit project!");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<int>> Delete(int id)
        {
            var result = await _context.DeleteProject(id);

            if (result > 0)
            {
                return Ok(result);
            }
            else
            {
                return NotFound("Could not delete applicant!");
            }
        }
    }
}
