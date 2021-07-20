using Microsoft.AspNetCore.Mvc;
using RecrutariBlazorWasm.Server.Extensions;
using RecrutariBlazorWasm.Server.Interfaces;
using RecrutariBlazorWasm.Shared.DTO;
using RecrutariBlazorWasm.Shared.Entities;
using RecrutariBlazorWasm.Shared.Helpers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RecrutariBlazorWasm.Server.Controllers
{
    public class ApplicantController : BaseApiController
    {
        private readonly IApplicantRepository _context;

        public ApplicantController(IApplicantRepository context)
        {
            this._context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Applicant>>> Get([FromQuery] PaginationParams paginationParams)
        {
            var result = await _context.GetApplicants(paginationParams);
            Response.AddPaginationHeader(paginationParams.PageNumber, paginationParams.PageSize, result.TotalItems);

            if (result != null)
            {
                return Ok(result.Items);
            }
            else
            {
                return NotFound("Could not get applicants!");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Applicant>> Get(int id)
        {
            var result = await _context.GetApplicantById(id);

            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                return NotFound("Could not find applicant!");
            }
        }

        [HttpGet("details/{id}")]
        public async Task<ActionResult<ApplicantDetailsDTO>> GetApplicantDetails(int id)
        {
            var result = await _context.GeApplicantDetails(id);

            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                return NotFound("Could not find applicant!");
            }
        }

        [HttpPost]
        public async Task<ActionResult<int>> Post(Applicant applicant)
        {
            applicant.LastUpdated = DateTime.Now;
            var result = await _context.CreateApplicant(applicant);
            if (result > 0)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest("Could not create applicant!");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<int>> Put(int id, ApplicantUpdateDTO applicant)
        {
            applicant.Applicant.LastUpdated = DateTime.Now;
            var result = await _context.UpdateApplicantDetails(id, applicant);

            if (result > 0)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest("Could not update applicant!");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<int>> Delete(int id)
        {
            var result = await _context.DeleteApplicant(id);

            if (result > 0)
            {
                return Ok(result);
            }
            else
            {
                return NotFound("Could not delete applicant!");
            }
        }

        [HttpGet("langqualproj")]
        public async Task<ActionResult<LanguageQualificationProjectList>> GetLanguageQualification()
        {

            var languageQualificationList = await _context.GetLanguageQualificationProject();

            return Ok(languageQualificationList);
        }
    }
}
