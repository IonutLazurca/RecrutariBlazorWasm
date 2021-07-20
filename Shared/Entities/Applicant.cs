using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RecrutariBlazorWasm.Shared.Entities
{
    public class Applicant
    {
        public Applicant()
        {
            Projects = new List<Project>();
            Languages = new List<LanguageQualification>();
        }
        public int Id { get; set; }

        [Required(ErrorMessage = "First Name is required")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Gender is required")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "Date Of Birth is required")]
        public DateTime? DateOfBirth { get; set; }

        [Required(ErrorMessage = "Nationality is required")]
        public string Nationality { get; set; }
        public string HomePhone { get; set; }

        [Required(ErrorMessage = "Mobile is required")]
        public string MobilePhone { get; set; }

        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "City is required")]
        public string City { get; set; }

        [Required(ErrorMessage = "Country is required")]
        public string Country { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? LastUpdated { get; set; }
        public virtual List<LanguageQualification> Languages { get; set; }
        public virtual List<Project> Projects { get; set; }
        public virtual ICollection<ApplicantProject> ApplicantProject { get; set; }
    }
}
