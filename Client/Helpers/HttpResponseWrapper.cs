using System.Net.Http;
using System.Threading.Tasks;

namespace RecrutariBlazorWasm.Client.Helpers
{
    public class HttpResponseWrapper<T>
    {
        public T Response { get; set; }
        public bool Success { get; set; }
        public HttpResponseMessage HttpResponseMessage { get; set; }

        public HttpResponseWrapper(T response, bool success, HttpResponseMessage httpResponseMessage)
        {
            this.Response = response;
            this.Success = success;
            this.HttpResponseMessage = httpResponseMessage;
        }

        public async Task<string> GetBody()
        {
            return await HttpResponseMessage.Content.ReadAsStringAsync();
        }
    }
}
