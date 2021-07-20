using RecrutariBlazorWasm.Client.Helpers;
using RecrutariBlazorWasm.Shared.Entities;
using RecrutariBlazorWasm.Shared.Helpers;
using System.Threading.Tasks;

namespace RecrutariBlazorWasm.Client.Interfaces
{
    public interface IProjectRepository
    {
        Task<int> CreateProject(Project project);
        Task<int> DeleteProject(int id);
        Task<Project> GetProjectById(int id);
        Task<PagingResponse<Project>> GetProjects(PaginationParams pagination);
        Task<int> UpdateProject(int id, Project project);
    }
}
