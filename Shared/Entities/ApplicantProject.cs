using System;
using System.ComponentModel.DataAnnotations;

namespace RecrutariBlazorWasm.Shared.Entities
{
    public class ApplicantProject
    {
        [Key]
        public int Id { get; set; }
        public int ApplicantId { get; set; }
        public int ProjectId { get; set; }
        public Applicant Applicant { get; set; }
        public Project Project { get; set; }
        public DateTime EnrollDate { get; set; }
        public DateTime CancelDate { get; set; }

        //public static ApplicantProject FromCSV(string csvLine)
        //{
        //    string[] values = csvLine.Split(';');
        //    ApplicantProject applicantproject = new ApplicantProject();
        //    applicantproject.EnrollDate = DateTime.Parse(values[0].ToString());
        //    applicantproject.CancelDate = DateTime.Parse(values[1].ToString());
        //    applicantproject.ApplicantId = Int32.Parse(values[2].ToString());

        //    return applicantproject;

        //}
    }
}
