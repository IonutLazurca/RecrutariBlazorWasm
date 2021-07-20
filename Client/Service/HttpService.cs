using Microsoft.AspNetCore.WebUtilities;
using RecrutariBlazorWasm.Client.Helpers;
using RecrutariBlazorWasm.Client.Interfaces;
using RecrutariBlazorWasm.Shared.Helpers;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace RecrutariBlazorWasm.Client.Service
{
    public class HttpService : IHttpService
    {
        private readonly HttpClient httpClient;
        private JsonSerializerOptions defaultJsonSerializerOption => new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        public HttpService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<HttpResponseWrapper<T>> Get<T>(string url)
        {
            var responseHttp = await httpClient.GetAsync(url);

            if (responseHttp.IsSuccessStatusCode)
            {
                var response = await DeserializeData<T>(responseHttp, defaultJsonSerializerOption);
                return new HttpResponseWrapper<T>(response, true, responseHttp);
            }
            return new HttpResponseWrapper<T>(default, false, responseHttp);
        }

        public async Task<HttpResponseWrapper<T>> Get<T>(string url, PaginationParams pagination)
        {
            var queryStringParam = new Dictionary<string, string>
            {
                ["pageNumber"] = pagination.PageNumber.ToString(),
                ["pageSize"] = pagination.PageSize.ToString()
            };
            var responseHttp = await httpClient.GetAsync(QueryHelpers.AddQueryString(url, queryStringParam));

            if (responseHttp.IsSuccessStatusCode)
            {
                var response = await DeserializeData<T>(responseHttp, defaultJsonSerializerOption);
                return new HttpResponseWrapper<T>(response, true, responseHttp);
            }
            return new HttpResponseWrapper<T>(default, false, responseHttp);
        }

        public async Task<HttpResponseWrapper<object>> Post<T>(string url, T data)
        {
            var responseHttp = await httpClient.PostAsync(url, data.SerializeData());

            return new HttpResponseWrapper<object>(null, responseHttp.IsSuccessStatusCode, responseHttp);
        }

        public async Task<HttpResponseWrapper<TResponse>> Post<T, TResponse>(string url, T data)
        {
            var responseHttp = await httpClient.PostAsync(url, data.SerializeData());

            if (responseHttp.IsSuccessStatusCode)
            {
                var responseDeserialized = await DeserializeData<TResponse>(responseHttp, defaultJsonSerializerOption);
                return new HttpResponseWrapper<TResponse>(responseDeserialized, true, responseHttp);
            }
            else
            {
                return new HttpResponseWrapper<TResponse>(default, false, responseHttp);
            }
        }

        public async Task<HttpResponseWrapper<object>> Put<T>(string url, T data)
        {
            var responseHttp = await httpClient.PutAsync(url, data.SerializeData());
            return new HttpResponseWrapper<object>(null, responseHttp.IsSuccessStatusCode, responseHttp);
        }

        public async Task<HttpResponseWrapper<object>> Delete(string url)
        {
            var responseHttp = await httpClient.DeleteAsync(url);
            return new HttpResponseWrapper<object>(null, responseHttp.IsSuccessStatusCode, responseHttp);
        }

        private async Task<T> DeserializeData<T>(HttpResponseMessage httpResponse, JsonSerializerOptions options)
        {
            var responseString = await httpResponse.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(responseString, options);
        }
    }
}
