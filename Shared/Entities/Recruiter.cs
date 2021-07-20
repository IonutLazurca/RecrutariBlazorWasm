using Microsoft.AspNetCore.Identity;

namespace RecrutariBlazorWasm.Shared.Entities
{
    public class Recruiter : IdentityUser
    {
        public string Name { get; set; }
    }
}
