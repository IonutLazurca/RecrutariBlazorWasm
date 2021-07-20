using RecrutariBlazorWasm.Shared.Entities;
using System.Threading.Tasks;

namespace RecrutariBlazorWasm.Server.Interfaces
{
    public interface ITokenService
    {
        Task<string> CreateToken(Recruiter recruiter);
    }
}
