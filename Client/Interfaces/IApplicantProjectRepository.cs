using RecrutariBlazorWasm.Shared.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RecrutariBlazorWasm.Client.Interfaces
{
    public interface IApplicantProjectRepository
    {
        Task<Applicant> GetApplicantIdWithProjects(int id);
        Task<List<Applicant>> GetApplicantsWithProjects();
        Task<Project> GetProjectIdWithApplicants(int id);
        Task<List<Project>> GetProjectsWithApplicants();
    }
}
