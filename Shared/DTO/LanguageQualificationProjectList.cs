using RecrutariBlazorWasm.Shared.Entities;
using System.Collections.Generic;

namespace RecrutariBlazorWasm.Shared.DTO
{
    public class LanguageQualificationProjectList
    {
        public List<MultipleSelector> LanguageList { get; set; }
        public List<MultipleSelector> QualificationList { get; set; }
        public List<Project> Projects { get; set; }
    }
}
