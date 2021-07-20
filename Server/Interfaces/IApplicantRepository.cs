using RecrutariBlazorWasm.Server.DTOs;
using RecrutariBlazorWasm.Shared.DTO;
using RecrutariBlazorWasm.Shared.Entities;
using RecrutariBlazorWasm.Shared.Helpers;
using System.Threading.Tasks;

namespace RecrutariBlazorWasm.Server.Interfaces
{
    public interface IApplicantRepository
    {
        Task<int> CreateApplicant(Applicant applicant);
        Task<int> DeleteApplicant(int Id);
        Task<ApplicantDetailsDTO> GeApplicantDetails(int id);
        Task<Applicant> GetApplicantById(int Id);
        Task<DapperQueryDTO<Applicant>> GetApplicants(PaginationParams pagination);
        Task<LanguageQualificationProjectList> GetLanguageQualificationProject();
        Task<int> UpdateApplicant(int Id, Applicant applicant);
        Task<int> UpdateApplicantDetails(int id, ApplicantUpdateDTO applicant);
    }
}
