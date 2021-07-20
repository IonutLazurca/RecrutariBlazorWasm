using RecrutariBlazorWasm.Client.Interfaces;
using RecrutariBlazorWasm.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace RecrutariBlazorWasm.Client.Repository
{
    public class ApplicantProjectRepository : IApplicantProjectRepository
    {
        private readonly HttpClient httpClient;

        private readonly JsonSerializerOptions defaultSerializerOptions = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };

        private const string url = "applicantproject";
        public ApplicantProjectRepository(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<List<Applicant>> GetApplicantsWithProjects()
        {
            var response = await httpClient.GetAsync($"{url}/getbyapplicants");
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }

            var applicants = JsonSerializer.Deserialize<List<Applicant>>(content, defaultSerializerOptions);

            return applicants;
        }

        public async Task<Applicant> GetApplicantIdWithProjects(int id)
        {
            var response = await httpClient.GetAsync($"{url}/getbyapplicants/{id}");
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }

            var applicant = JsonSerializer.Deserialize<Applicant>(content, defaultSerializerOptions);

            return applicant;
        }

        public async Task<List<Project>> GetProjectsWithApplicants()
        {
            var response = await httpClient.GetAsync($"{url}/getbyprojects");
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }

            var projects = JsonSerializer.Deserialize<List<Project>>(content, defaultSerializerOptions);

            return projects;
        }

        public async Task<Project> GetProjectIdWithApplicants(int id)
        {
            var response = await httpClient.GetAsync($"{url}/getbyproject/{id}s");
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }

            var project = JsonSerializer.Deserialize<Project>(content, defaultSerializerOptions);

            return project;
        }
    }
}
