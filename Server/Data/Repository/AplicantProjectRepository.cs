using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;
using RecrutariBlazorWasm.Server.Interfaces;
using RecrutariBlazorWasm.Shared.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecrutariBlazorWasm.Server.Data.Repository
{
    public class AplicantProjectRepository : IAplicantProjectRepository
    {
        public string connection { get; set; }

        public AplicantProjectRepository(IConfiguration configuration)
        {
            connection = configuration.GetConnectionString("PostgresqlConnection");
        }

        public async Task<IEnumerable<Applicant>> GetApplicantsWithProjects()
        {
            using (var cnn = new NpgsqlConnection(connection))
            {
                var sql = @"SELECT a.*, ap.*, p.* FROM ""Applicants"" AS a JOIN ""ApplicantsProjects"" AS ap ON a.""Id"" = ap.""ApplicantId"" JOIN ""Projects"" AS p ON ap.""ProjectId"" = p.""Id"" ";

                var applicantDic = new Dictionary<int, Applicant>();
                var applicants = await cnn.QueryAsync<Applicant, ApplicantProject, Project, Applicant>(sql, (a, ap, p) =>
                {
                    if (!applicantDic.TryGetValue(a.Id, out var currentApplicant))
                    {
                        currentApplicant = a;
                        applicantDic.Add(currentApplicant.Id, currentApplicant);
                    }
                    currentApplicant.Projects.Add(p);
                    return currentApplicant;

                }, splitOn: "Id,Id");
                return applicants.Distinct().ToList();
            }
        }

        public async Task<Applicant> GetApplicantIdWithProjects(int id)
        {
            using (var cnn = new NpgsqlConnection(connection))
            {
                var parameters = new DynamicParameters();
                parameters.Add("Id", id);
                var sql = @"SELECT a.*, ap.*, p.* FROM ""Applicants"" AS a JOIN ""ApplicantsProjects"" AS ap ON a.""Id"" = ap.""ApplicantId"" JOIN ""Projects"" AS p ON ap.""ProjectId"" = p.""Id"" WHERE a.""Id"" = @Id";

                var applicantDic = new Dictionary<int, Applicant>();
                var applicant = await cnn.QueryAsync<Applicant, ApplicantProject, Project, Applicant>(sql, (a, ap, p) =>
                {
                    if (!applicantDic.TryGetValue(a.Id, out var currentApplicant))
                    {
                        currentApplicant = a;
                        applicantDic.Add(currentApplicant.Id, currentApplicant);
                    }
                    currentApplicant.Projects.Add(p);
                    return currentApplicant;

                }, parameters, splitOn: "Id,Id");
                return applicant.FirstOrDefault();
            }
        }

        public async Task<IEnumerable<Project>> GetProjectsWithApplicants()
        {
            using (var cnn = new NpgsqlConnection(connection))
            {
                var sql = @"SELECT p.*, ap.*, a.* FROM ""Projects"" AS p JOIN ""ApplicantsProjects"" AS ap ON p.""Id"" = ap.""ProjectId"" JOIN ""Applicants"" AS a ON ap.""ApplicantId"" = a.""Id"";  ";

                var projectDic = new Dictionary<int, Project>();
                var projects = await cnn.QueryAsync<Project, ApplicantProject, Applicant, Project>(sql, (p, ap, a) =>
                {
                    if (!projectDic.TryGetValue(p.Id, out var currentProject))
                    {
                        currentProject = p;
                        projectDic.Add(currentProject.Id, currentProject);
                    }
                    currentProject.Applicants.Add(a);
                    return currentProject;

                }, splitOn: "Id,Id");
                return projects.Distinct().ToList();
            }
        }

        public async Task<Project> GetProjectIdWithApplicants(int id)
        {
            using (var cnn = new NpgsqlConnection(connection))
            {
                var parameters = new DynamicParameters();
                parameters.Add("Id", id);
                var sql = @"SELECT p.*, ap.*, a.* FROM ""Projects"" AS p JOIN ""ApplicantsProjects"" AS ap ON p.""Id"" = ap.""ProjectId"" JOIN ""Applicants"" AS a ON ap.""ApplicantId"" = a.""Id"" WHERE p.""Id"" = @Id;  ";

                var projectDic = new Dictionary<int, Project>();
                var project = await cnn.QueryAsync<Project, ApplicantProject, Applicant, Project>(sql, (p, ap, a) =>
                {
                    if (!projectDic.TryGetValue(p.Id, out var currentProject))
                    {
                        currentProject = p;
                        projectDic.Add(currentProject.Id, currentProject);
                    }
                    currentProject.Applicants.Add(a);
                    return currentProject;

                }, parameters, splitOn: "Id,Id");
                return project.FirstOrDefault();
            }
        }
    }
}
