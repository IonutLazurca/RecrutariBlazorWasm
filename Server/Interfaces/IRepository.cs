using RecrutariBlazorWasm.Server.DTOs;
using RecrutariBlazorWasm.Shared.Helpers;
using System.Threading.Tasks;

namespace RecrutariBlazorWasm.Server.Interfaces
{
    public interface IRepository<T>
    {
        Task<int> Create(T state);
        Task<int> Delete(int Id);
        Task<T> GetById(int Id);
        Task<DapperQueryDTO<T>> Get(PaginationParams pagination);
        Task<int> Update(int Id, T state);
    }
}
