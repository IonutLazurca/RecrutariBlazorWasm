using RecrutariBlazorWasm.Shared.Entities;
using System.Collections.Generic;

namespace RecrutariBlazorWasm.Shared.DTO
{
    public class ApplicantDetailsDTO
    {
        public Applicant Applicant { get; set; }
        public List<MultipleSelector> Languages { get; set; }
        public List<MultipleSelector> Qualifications { get; set; }
        public List<Project> AllProjects { get; set; }
        public List<Holiday> Dates { get; set; }
        public List<LoggedApplicants> LoggedApplicants { get; set; }
    }
}
