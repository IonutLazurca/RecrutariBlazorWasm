using RecrutariBlazorWasm.Shared.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RecrutariBlazorWasm.Server.Interfaces
{
    public interface IAplicantProjectRepository
    {
        Task<Applicant> GetApplicantIdWithProjects(int id);
        Task<IEnumerable<Applicant>> GetApplicantsWithProjects();
        Task<Project> GetProjectIdWithApplicants(int id);
        Task<IEnumerable<Project>> GetProjectsWithApplicants();
    }
}
