using Microsoft.AspNetCore.WebUtilities;
using RecrutariBlazorWasm.Client.Helpers;
using RecrutariBlazorWasm.Client.Interfaces;
using RecrutariBlazorWasm.Shared.DTO;
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
    public class ApplicantRepository : IApplicantRepository
    {
        private readonly HttpClient httpClient;
        private string url = "applicant";

        private JsonSerializerOptions defaultJsonSerializerOption => new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        public ApplicantRepository(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<Applicant> GetApplicantById(int id)
        {
            var response = await httpClient.GetAsync($"{url}/{id}");
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }

            var applicant = JsonSerializer.Deserialize<Applicant>(content, defaultJsonSerializerOption);

            return applicant;
        }

        public async Task<LanguageQualificationProjectList> GetLanguageQualificationList()
        {
            var response = await httpClient.GetAsync($"{url}/langqualproj");
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }
            var languageQualificationProject = JsonSerializer.Deserialize<LanguageQualificationProjectList>(content, defaultJsonSerializerOption);

            return languageQualificationProject;
        }

        public async Task<ApplicantDetailsDTO> GetApplicantDetails(int id)
        {
            var response = await httpClient.GetAsync($"{url}/details/{id}");
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }

            var applicant = JsonSerializer.Deserialize<ApplicantDetailsDTO>(content, defaultJsonSerializerOption);

            return applicant;
        }

        public async Task<int> CreateApplicant(Applicant applicant)
        {
            var response = await httpClient.PostAsync(url, applicant.SerializeData());
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }

            return 1;
        }

        public async Task<int> UpdateApplicant(int id, Applicant applicant)
        {
            var response = await httpClient.PutAsync($"{url}/{id}", applicant.SerializeData());
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }

            return 1;
        }

        public async Task<int> UpdateApplicantDetails(int id, ApplicantUpdateDTO applicant)
        {
            var response = await httpClient.PutAsync($"{url}/{id}", applicant.SerializeData());
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }

            return 1;
        }

        public async Task<int> DeleteApplicant(int id)
        {
            var response = await httpClient.DeleteAsync($"{url}/{id}");
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }

            return 1;
        }

        public async Task<PagingResponse<Applicant>> GetApplicants(PaginationParams pagination)
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

            var pagingResponse = new PagingResponse<Applicant>
            {
                Items = JsonSerializer.Deserialize<List<Applicant>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }),
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
