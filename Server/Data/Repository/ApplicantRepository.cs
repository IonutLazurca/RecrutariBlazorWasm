using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;
using RecrutariBlazorWasm.Server.DTOs;
using RecrutariBlazorWasm.Server.Interfaces;
using RecrutariBlazorWasm.Shared.DTO;
using RecrutariBlazorWasm.Shared.Entities;
using RecrutariBlazorWasm.Shared.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace RecrutariBlazorWasm.Server.Data.Repository
{
    public class ApplicantRepository : IApplicantRepository
    {
        public string connection { get; set; }

        public ApplicantRepository(IConfiguration configuration)
        {
            connection = configuration.GetConnectionString("PostgresqlConnection");
        }

        public async Task<int> CreateApplicant(Applicant applicant)
        {
            if (applicant != null)
            {
                try
                {
                    using (var cnn = new NpgsqlConnection(connection))
                    {
                        /////
                        //var parameters = new DynamicParameters(applicant);
                        //parameters.Output(applicant, x => x.Id);
                        /////
                        //var parameters = new DynamicParameters();
                        //parameters.Add("@Id", null, DbType.Int32, direction: ParameterDirection.Output);
                        //parameters.Add("@FirstName", applicant.FirstName, DbType.String);
                        //parameters.Add("@LastName", applicant.LastName, DbType.String);
                        //parameters.Add("@Gender", applicant.Gender, DbType.String);
                        //parameters.Add("@DateOfBirth", applicant.DateOfBirth);
                        //parameters.Add("@Nationality", applicant.Nationality, DbType.String);
                        //parameters.Add("@HomePhone", applicant.HomePhone, DbType.String);
                        //parameters.Add("@MobilePhone", applicant.MobilePhone, DbType.String);
                        //parameters.Add("@Email", applicant.Email, DbType.String);
                        //parameters.Add("@City", applicant.City, DbType.String);
                        //parameters.Add("@Country", applicant.Country, DbType.String);
                        //parameters.Add("@LastUpdated", applicant.LastUpdated);
                        var sql = @"INSERT INTO ""Applicants"" " +
                                     @"(""FirstName"", ""LastName"", ""Gender"", ""DateOfBirth"", ""Nationality"", ""HomePhone"", " +
                                     @"""MobilePhone"", ""Email"", ""City"", ""Country"", ""LastUpdated"") VALUES(" +
                                     @"@FirstName, @LastName, @Gender, @DateOfBirth, @Nationality, @HomePhone, @MobilePhone, @Email, " +
                                     @"@City, @Country, @LastUpdated)" +
                                     @"RETURNING ""Id""";
                        var res = await cnn.ExecuteScalarAsync<int>(sql, applicant);

                        if(res > 0)
                        {
                            if(applicant.Projects != null && applicant.Projects.Any())
                            {
                                List<ApplicantProject> newProjects = new List<ApplicantProject>();
                                foreach (var item in applicant.Projects)
                                {
                                    newProjects.Add(new ApplicantProject {ProjectId = item.Id, ApplicantId = res, EnrollDate = DateTime.Now });
                                }

                                var sqlProjects = @"INSERT INTO ""ApplicantsProjects"" (""ApplicantId"", ""ProjectId"", ""EnrollDate"") VALUES(@ApplicantId, @ProjectId, @EnrollDate)";
                                var responseProjects = await cnn.ExecuteAsync(sqlProjects, newProjects);
                            }
                            if(applicant.Languages != null && applicant.Languages.Any())
                            {
                                applicant.Languages.Select(x => { x.ApplicantId = res; return x; }).ToList();
                                var sqlLanguageQualification = @"INSERT INTO ""LanguagesQualifications"" (""ApplicantId"",""LanguageId"",""LanguageName"", ""QualificationId"",""QualificationName"")" +
                                                                @" VALUES(@ApplicantId, @LanguageId, @LanguageName, @QualificationId, @QualificationName)";
                                var responseLanguages = await cnn.ExecuteAsync(sqlLanguageQualification, applicant.Languages);
                            }
                        }
                        return res;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    throw;
                }
            }
            return -1;
        }

        public async Task<int> DeleteApplicant(int Id)
        {
            if (Id > 0)
            {
                try
                {
                    using (var cnn = new NpgsqlConnection(connection))
                    {
                        var parameters = new DynamicParameters();
                        parameters.Add("id", Id);
                        var sql = @"DELETE FROM ""Applicants"" WHERE ""Id"" = @id";
                        var user = await cnn.ExecuteAsync(sql, parameters);
                        return 1;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    throw;
                }
            }
            else
            {
                return -1;
            }

        }

        public async Task<Applicant> GetApplicantById(int Id)
        {
            try
            {
                using (var cnn = new NpgsqlConnection(connection))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("id", Id);
                    var sql = @"SELECT * FROM ""Applicants"" WHERE ""Id"" = @id";
                    var user = await cnn.QueryAsync<Applicant>(sql, parameters);
                    var res = user.SingleOrDefault();
                    return res;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<ApplicantDetailsDTO> GeApplicantDetails(int id)
        {
            using (var cnn = new NpgsqlConnection(connection))
            {
                var parameters = new DynamicParameters();
                parameters.Add("Id", id);
                var sql = @"SELECT a.*, ap.*, p.*, l.* FROM ""Applicants"" AS a LEFT JOIN ""ApplicantsProjects"" AS ap ON a.""Id"" = ap.""ApplicantId"" LEFT JOIN ""Projects"" AS p ON ap.""ProjectId"" = p.""Id"" LEFT JOIN ""LanguagesQualifications"" AS l ON l.""ApplicantId"" = a.""Id"" WHERE a.""Id"" = @Id ";

                var applicantDic = new Dictionary<int, Applicant>();
                var languageDic = new Dictionary<int, LanguageQualification>();
                var projectDic = new Dictionary<int, Project>();
                var applicant = await cnn.QueryAsync<Applicant, ApplicantProject, Project, LanguageQualification, Applicant>(sql, (a, ap, p, l) =>
                {
                    if (!applicantDic.TryGetValue(a.Id, out var applicantQuery))
                    {
                        applicantQuery = a;
                        applicantDic.Add(applicantQuery.Id, applicantQuery);
                    }

                    if (p != null && !projectDic.TryGetValue(p.Id, out var currentProject))
                    {
                        currentProject = p;
                        projectDic.Add(currentProject.Id, currentProject);
                        applicantQuery.Projects.Add(currentProject);
                    }

                    if (l != null && !languageDic.TryGetValue(l.Id, out var currentLanguage))
                    {
                        currentLanguage = l;
                        languageDic.Add(currentLanguage.Id, currentLanguage);
                        applicantQuery.Languages.Add(l);
                    }

                    return applicantQuery;

                }, parameters, splitOn: "Id,Id,Id");

                List<MultipleSelector> languageList;
                List<MultipleSelector> qualificationList;
                List<Project> projects;
                List<Holiday> dates;
                List<LoggedApplicants> loggedApplicants;

                var sqlLanguageQualification = @"SELECT * FROM ""LanguageNames""; " +
                        @"SELECT * FROM ""QualificationNames""; SELECT ""Id"", ""Name"", ""Company"" FROM ""Projects""; SELECT ""Title"", ""Date"" FROM ""Holidays""; SELECT proj.""Name"", COUNT(*) AS ""TotalApplicants"" FROM (SELECT ap.*, p.* FROM ""ApplicantsProjects"" AS ap JOIN ""Projects"" AS p ON ap.""ProjectId"" = p.""Id"") AS proj GROUP BY proj.""Name""; ";

                using (var lists = await cnn.QueryMultipleAsync(sqlLanguageQualification))
                {
                    languageList = lists.Read<MultipleSelector>().ToList();
                    qualificationList = lists.Read<MultipleSelector>().ToList();
                    projects = lists.Read<Project>().ToList();
                    dates = lists.Read<Holiday>().ToList();
                    loggedApplicants = lists.Read<LoggedApplicants>().ToList();
                }

                var applicantProjects = applicant.FirstOrDefault();
                var idsProjects = applicantProjects.Projects.Select(x => x.Id);
                var idsTotalProjects = projects.Select(x => x.Id);
                var idsProjectsAvailable = idsTotalProjects.Except(idsProjects).ToList();

                var sqlProjectsAvailable = @"SELECT ""Id"", ""Name"", ""Company"", ""StartDate"", ""EndDate"" FROM ""Projects"" WHERE ""Id"" = ANY(@Projects) ORDER BY ""Id"" ASC;";
                var parametersProject = new DynamicParameters();
                parametersProject.Add("Projects", idsProjectsAvailable);

                var projectsAvailable = await cnn.QueryAsync<Project>(sqlProjectsAvailable, parametersProject);

                return new ApplicantDetailsDTO()
                {
                    Applicant = applicant.FirstOrDefault(),
                    Languages = languageList,
                    Qualifications = qualificationList,
                    AllProjects = projectsAvailable.ToList(),
                    Dates = dates,
                    LoggedApplicants = loggedApplicants
                };
            }
        }

        public async Task<int> UpdateApplicantDetails(int id, ApplicantUpdateDTO applicant)
        {
            if (id != applicant.Applicant.Id)
            {
                return -1;
            }
            // 1) optimize sql update on Projects by checking any changes between initial load state and update state.
            // 2) optimize sql update on LanguageQualification by checking any changes between initial load state and update state.


            //Update Applicant
            using (var cnn = new NpgsqlConnection(connection))
            {
                var sqlApplicant = @"UPDATE ""Applicants"" SET " +
                                        @"""FirstName"" = @FirstName, " +
                                        @"""LastName"" = @LastName, " +
                                        @"""Gender"" = @Gender, " +
                                        @"""DateOfBirth"" = @DateOfBirth, " +
                                        @"""Nationality"" = @Nationality, " +
                                        @"""HomePhone"" = @HomePhone, " +
                                        @"""MobilePhone"" = @MobilePhone, " +
                                        @"""Email"" = @Email, " +
                                        @"""City"" = @City, " +
                                        @"""Country"" = @Country, " +
                                        @"""LastUpdated"" = @LastUpdated " +
                                        @"WHERE (""Id"" = @Id) " +
                                        @"RETURNING *";

                var res = await cnn.ExecuteAsync(sqlApplicant, applicant.Applicant);

            }

            //Update Project
            if (applicant.InitialProjects != null)
            {
                var initialProjects = applicant.InitialProjects;
                var idsProjectInitials = initialProjects.Select(x => x.Id).ToList();
                var idsProjectFinals = applicant.Applicant.Projects.Select(x => x.Id).ToList();

                var idsProjectToInsert = idsProjectFinals.Except(idsProjectInitials).ToList();
                var idsProjectToDelete = idsProjectInitials.Except(idsProjectFinals).ToList();

                if (idsProjectToDelete != null || idsProjectToInsert.Any())
                {
                    using (var cnn = new NpgsqlConnection(connection))
                    {
                        var sqlDeleteProjects = @"DELETE FROM ""ApplicantsProjects"" WHERE ""ApplicantId"" = @Id AND ""ProjectId"" = ANY(@ProjectList) ;";
                        var parameters = new DynamicParameters();
                        parameters.Add("Id", applicant.Applicant.Id);
                        parameters.Add("ProjectList", idsProjectToDelete);
                        var resProj = await cnn.ExecuteAsync(sqlDeleteProjects, parameters);

                        #region //Creare obiect cu scopul BulkInsert.
                        //List<ApplicantProject> applicantProject = new List<ApplicantProject>();
                        //foreach (var item in idsToInsert)
                        //{
                        //    var appProj = new ApplicantProject() { ApplicantId = applicant.Id, ProjectId = item };
                        //    applicantProject.Add(appProj);
                        //}
                        #endregion

                        foreach (var item in idsProjectToInsert)
                        {
                            var sqlToInsert = @"INSERT INTO ""ApplicantsProjects"" (""ApplicantId"", ""ProjectId"") VALUES (@ApplicantId, @ProjectId)";
                            var parametersBulkInsert = new DynamicParameters();
                            parametersBulkInsert.Add("ApplicantId", applicant.Applicant.Id);
                            parametersBulkInsert.Add("ProjectId", item);
                            var insertResult = await cnn.ExecuteAsync(sqlToInsert, parametersBulkInsert);
                        }
                    }
                }
            }


            using (var cnn = new NpgsqlConnection(connection))
            {
                var sqlLanguageQualificationDelete = @"DELETE FROM ""LanguagesQualifications"" WHERE ""ApplicantId"" = @ApplicantId";
                await cnn.ExecuteAsync(sqlLanguageQualificationDelete, new { ApplicantId = applicant.Applicant.Id });

                var sqlLanguageQualificationToInsert = @"INSERT INTO ""LanguagesQualifications"" (""LanguageName"", ""QualificationName"", ""LanguageId"", ""QualificationId"", ""ApplicantId"") VALUES (@LanguageName, @QualificationName, @LanguageId, @QualificationId, @ApplicantId)";

                foreach (var item in applicant.Applicant.Languages)
                {
                    await cnn.ExecuteAsync(sqlLanguageQualificationToInsert, new { LanguageName = item.LanguageName, QualificationName = item.QualificationName, LanguageId = item.LanguageId, QualificationId = item.QualificationId, ApplicantId = item.ApplicantId });
                }
            }

            //Update LanguagesQualifications  - nu a mers. Am avut o eroare de Dapper si MapSql. Zicea ceva de parameterless default pentru LanguageQualification class. Adica nu m-a lasat sa implementez IEquatable<LanguageQualification>

            //var initialLanguageQualification = applicant.InitialLanguageQualification;
            //var finalLanguageQualification = applicant.Applicant.Languages;

            //var languageQualificationToInsert = new List<LanguageQualification>();
            //var languageQualificationToDelete = new List<LanguageQualification>();

            //foreach (var item in finalLanguageQualification)
            //{
            //    if (!initialLanguageQualification.Contains(item))
            //    {
            //        languageQualificationToInsert.Add(item);
            //    }
            //}

            //foreach (var item in initialLanguageQualification)
            //{
            //    if (!finalLanguageQualification.Contains(item))
            //    {
            //        languageQualificationToDelete.Add(item);
            //    }
            //}

            //if (!finalLanguageQualification.All(x => initialLanguageQualification.Contains(x)) ||
            //       languageQualificationToInsert.Any() || languageQualificationToDelete.Any() )
            //{
            //    using (var cnn = new NpgsqlConnection(connection))
            //    {
            //        var sqlLanguageDelete = @"DELETE FROM ""LanguagesQualifications"" WHERE ""ApplicantId"" = @ApplicantId";
            //        await cnn.ExecuteAsync(sqlLanguageDelete, new { ApplicantId = applicant.Applicant.Id });

            //        var sqlLanguageToInsert = @"INSERT INTO ""LanguagesQualifications"" (""LanguageName"", ""QualificationName"", ""LanguageId"", ""QualificationId"", ""ApplicantId"") VALUES (@LanguageName, @QualificationName, @LanguageId, @QualificationId, @ApplicantId)";
            //        foreach (var item in finalLanguageQualification)
            //        {
            //            await cnn.ExecuteAsync(sqlLanguageToInsert, new { LanguageName = item.LanguageName, QualificationName = item.QualificationName, LanguageId = item.LanguageName, QualificationId = item.QualificationName, ApplicantId = item.ApplicantId });
            //        }
            //    }
            //}
            return 1;
        }

        public async Task<DapperQueryDTO<Applicant>> GetApplicants(PaginationParams pagination)
        {
            try
            {
                using (var cnn = new NpgsqlConnection(connection))
                {
                    var offset = (pagination.PageNumber - 1) * pagination.PageSize;
                    var parameters = new DynamicParameters();
                    parameters.Add("Offset", offset);
                    parameters.Add("Limit", pagination.PageSize);
                    var sql = @"SELECT * FROM ""Applicants"" ORDER BY ""Id"" OFFSET @Offset LIMIT @Limit ";
                    var sqlCount = @"SELECT COUNT(*) FROM ""Applicants""";

                    var applicants = await cnn.QueryAsync<Applicant>(sql, parameters);
                    var count = await cnn.QueryAsync<int>(sqlCount);
                    int countValue = count.Single();

                    return new DapperQueryDTO<Applicant>(applicants, countValue);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<int> UpdateApplicant(int Id, Applicant applicant)
        {
            if (Id == applicant.Id)
            {
                try
                {
                    using (var cnn = new NpgsqlConnection(connection))
                    {
                        var sql = @"UPDATE ""Applicants"" SET " +
                                        @"""FirstName"" = @FirstName, " +
                                        @"""LastName"" = @LastName, " +
                                        @"""Gender"" = @Gender, " +
                                        @"""DateOfBirth"" = @DateOfBirth, " +
                                        @"""Nationality"" = @Nationality, " +
                                        @"""HomePhone"" = @HomePhone, " +
                                        @"""MobilePhone"" = @MobilePhone, " +
                                        @"""Email"" = @Email, " +
                                        @"""City"" = @City, " +
                                        @"""Country"" = @Country, " +
                                        @"""LastUpdated"" = @LastUpdated " +
                                        @"WHERE (""Id"" = @Id) " +
                                        @"RETURNING *";
                        var res = (int)await cnn.ExecuteAsync(sql, applicant);
                        return res;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    throw;
                }
            }
            return -1;
        }

        public async Task<LanguageQualificationProjectList> GetLanguageQualificationProject()
        {
            List<MultipleSelector> languageList;
            List<MultipleSelector> qualificationList;
            List<Project> projects;

            using (var cnn = new NpgsqlConnection(connection))
            {
                var sql = @"SELECT * FROM ""LanguageNames""; SELECT * FROM ""QualificationNames""; SELECT * FROM ""Projects""";

                using (var lists = await cnn.QueryMultipleAsync(sql))
                {
                    languageList = lists.Read<MultipleSelector>().ToList();
                    qualificationList = lists.Read<MultipleSelector>().ToList();
                    projects = lists.Read<Project>().ToList();
                }
            }

            return new LanguageQualificationProjectList { LanguageList = languageList, QualificationList = qualificationList, Projects = projects };
        }
    }
}
