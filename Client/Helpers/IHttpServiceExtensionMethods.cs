using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace RecrutariBlazorWasm.Client.Helpers
{
    public static class IHttpServiceExtensionMethods
    {
        public static StringContent SerializeData<T>(this T data)
        {
            var dataJson = JsonSerializer.Serialize(data);
            var stringContent = new StringContent(dataJson, Encoding.UTF8, "application/json");

            return stringContent;
        }
    }
}
