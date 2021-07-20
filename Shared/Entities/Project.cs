using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RecrutariBlazorWasm.Shared.Entities
{
    public class Project
    {
        public Project()
        {
            Applicants = new List<Applicant>();
        }
        public Project(int id, string name, string company)
        {
            this.Id = id;
            this.Name = name;
            this.Company = company;
        }
        public int Id { get; set; }

        [Required(ErrorMessage = "Project name required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Company required")]
        public string Company { get; set; }

        //[Required(ErrorMessage = "Description required")]
        public string Description { get; set; }

        //[Required(ErrorMessage = "Start Date required")]
        public DateTime? StartDate { get; set; }

        //[Required(ErrorMessage = "End Date name required")]
        public DateTime? EndDate { get; set; }

        public DateTime? DateCreated { get; set; }
        public DateTime? LastUpdated { get; set; }
        public int LoggedApplicants { get; set; }
        public virtual List<Applicant> Applicants { get; set; }
        public virtual ICollection<ApplicantProject> ApplicantProject { get; set; }
    }
}
