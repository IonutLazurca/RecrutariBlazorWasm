using RecrutariBlazorWasm.Client.Helpers;
using RecrutariBlazorWasm.Shared.Helpers;
using System.Threading.Tasks;

namespace RecrutariBlazorWasm.Client.Interfaces
{
    public interface IHttpService
    {
        Task<HttpResponseWrapper<T>> Get<T>(string url);
        Task<HttpResponseWrapper<object>> Delete(string url);
        Task<HttpResponseWrapper<object>> Post<T>(string url, T data);
        Task<HttpResponseWrapper<TResponse>> Post<T, TResponse>(string url, T data);
        Task<HttpResponseWrapper<object>> Put<T>(string url, T data);
        Task<HttpResponseWrapper<T>> Get<T>(string url, PaginationParams pagination);
    }
}
