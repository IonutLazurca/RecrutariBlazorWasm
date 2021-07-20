using Microsoft.AspNetCore.Mvc;
using RecrutariBlazorWasm.Server.Interfaces;
using RecrutariBlazorWasm.Shared.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RecrutariBlazorWasm.Server.Controllers
{
    public class ApplicantProjectController : BaseApiController
    {
        private readonly IAplicantProjectRepository _context;

        public ApplicantProjectController(IAplicantProjectRepository context)
        {
            this._context = context;
        }

        [HttpGet("getbyapplicants")]
        public async Task<ActionResult<IEnumerable<Applicant>>> GetByApplicants()
        {
            var result = await _context.GetApplicantsWithProjects();
            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest("Could not retrieve requested data");
            }
        }

        [HttpGet("getbyapplicants/{id}")]
        public async Task<ActionResult<Applicant>> GetByApplicants(int id)
        {
            var result = await _context.GetApplicantIdWithProjects(id);
            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest("Could not retrieve requested data");
            }
        }

        [HttpGet("getbyprojects")]
        public async Task<ActionResult<IEnumerable<Project>>> GetByProjects()
        {
            var result = await _context.GetProjectsWithApplicants();
            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest("Could not retrieve requested data");
            }
        }

        [HttpGet("getbyprojects/{id}")]
        public async Task<ActionResult<Project>> GetByProjects(int id)
        {
            var result = await _context.GetProjectIdWithApplicants(id);
            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest("Could not retrieve requested data");
            }
        }
    }
}
