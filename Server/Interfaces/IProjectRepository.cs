using RecrutariBlazorWasm.Server.DTOs;
using RecrutariBlazorWasm.Shared.Entities;
using RecrutariBlazorWasm.Shared.Helpers;
using System.Threading.Tasks;

namespace RecrutariBlazorWasm.Server.Interfaces
{
    public interface IProjectRepository
    {
        Task<int> CreateProject(Project project);
        Task<int> DeleteProject(int Id);
        Task<Project> GetProjectById(int Id);
        Task<DapperQueryDTO<Project>> GetProjects(PaginationParams pagination);
        Task<int> UpdateProject(int Id, Project project);
    }
}
