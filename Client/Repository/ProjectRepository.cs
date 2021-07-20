using Microsoft.AspNetCore.WebUtilities;
using RecrutariBlazorWasm.Client.Helpers;
using RecrutariBlazorWasm.Client.Interfaces;
using RecrutariBlazorWasm.Shared.Entities;
using RecrutariBlazorWasm.Shared.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace RecrutariBlazorWasm.Client.Repository
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly HttpClient httpClient;
        private string url = "project";

        private JsonSerializerOptions defaultJsonSerializerOption => new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        public ProjectRepository(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<Project> GetProjectById(int id)
        {
            var response = await httpClient.GetAsync($"{url}/{id}");
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }

            var Project = JsonSerializer.Deserialize<Project>(content, defaultJsonSerializerOption);

            return Project;

        }

        public async Task<int> CreateProject(Project project)
        {
            var response = await httpClient.PostAsync(url, project.SerializeData());
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }

            return 1;
        }

        public async Task<int> UpdateProject(int id, Project project)
        {
            var response = await httpClient.PutAsync($"{url}/{id}", project.SerializeData());
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }

            return 1;

        }

        public async Task<int> DeleteProject(int id)
        {
            var response = await httpClient.DeleteAsync($"{url}/{id}");
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }

            return 1;
        }

        public async Task<PagingResponse<Project>> GetProjects(PaginationParams pagination)
        {
            var queryStringParam = new Dictionary<string, string>
            {
                ["pageNumber"] = pagination.PageNumber.ToString(),
                ["pageSize"] = pagination.PageSize.ToString()
            };
            var response = await httpClient.GetAsync(QueryHelpers.AddQueryString(url, queryStringParam));

            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }

            var pagingResponse = new PagingResponse<Project>
            {
                Items = JsonSerializer.Deserialize<List<Project>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }),
                Metadata = JsonSerializer.Deserialize<MetaData>(response.Headers.GetValues("X-Pagination").First(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true })

            };

            return pagingResponse;
        }

        private async Task<T> Deserialize<T>(HttpResponseMessage httpResponseMessage, JsonSerializerOptions options)
        {
            var contentString = await httpResponseMessage.Content.ReadAsStringAsync();
            var contentObject = JsonSerializer.Deserialize<T>(contentString, options);
            return contentObject;
        }

    }
}
