using System.ComponentModel.DataAnnotations;

namespace RecrutariBlazorWasm.Shared.DTO
{
    public class AuthenticationDTO
    {
        [Required(ErrorMessage = "Email required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password required")]
        public string Password { get; set; }
    }
}
