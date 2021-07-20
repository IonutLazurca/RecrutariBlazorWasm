using RecrutariBlazorWasm.Shared.Entities;
using System.Collections.Generic;

namespace RecrutariBlazorWasm.Shared.DTO
{
    public class ApplicantUpdateDTO
    {
        public Applicant Applicant { get; set; }
        public List<Project> InitialProjects { get; set; }
        public List<LanguageQualification> InitialLanguageQualification { get; set; }
    }
}
