using RecrutariBlazorWasm.Client.Helpers;
using RecrutariBlazorWasm.Shared.DTO;
using RecrutariBlazorWasm.Shared.Entities;
using RecrutariBlazorWasm.Shared.Helpers;
using System.Threading.Tasks;

namespace RecrutariBlazorWasm.Client.Interfaces
{
    public interface IApplicantRepository
    {
        Task<Applicant> GetApplicantById(int id);
        Task<int> UpdateApplicant(int id, Applicant applicant);
        Task<int> CreateApplicant(Applicant applicant);
        Task<int> DeleteApplicant(int id);
        Task<PagingResponse<Applicant>> GetApplicants(PaginationParams pagination);
        Task<ApplicantDetailsDTO> GetApplicantDetails(int id);
        Task<int> UpdateApplicantDetails(int id, ApplicantUpdateDTO applicant);
        Task<LanguageQualificationProjectList> GetLanguageQualificationList();
    }
}
