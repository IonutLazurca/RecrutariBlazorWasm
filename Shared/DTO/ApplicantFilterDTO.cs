using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecrutariBlazorWasm.Shared.DTO
{
    public class ApplicantFilterDTO
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Gender { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string Nationality { get; set; }

        public string HomePhone { get; set; }

        public string MobilePhone { get; set; }

        public string Email { get; set; }

    }
}
