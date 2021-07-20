using System.Collections.Generic;

namespace RecrutariBlazorWasm.Shared.DTO
{
    public class RegistrationResponseDTO
    {
        public bool IsRegistrationSuccessful { get; set; }
        public IEnumerable<string> Errors { get; set; }
    }
}
