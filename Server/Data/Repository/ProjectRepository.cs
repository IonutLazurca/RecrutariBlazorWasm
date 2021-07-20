using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;
using RecrutariBlazorWasm.Server.DTOs;
using RecrutariBlazorWasm.Server.Interfaces;
using RecrutariBlazorWasm.Shared.Entities;
using RecrutariBlazorWasm.Shared.Helpers;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace RecrutariBlazorWasm.Server.Data.Repository
{
    public class ProjectRepository : IProjectRepository
    {
        public string connection { get; set; }

        public ProjectRepository(IConfiguration configuration)
        {
            connection = configuration.GetConnectionString("PostgresqlConnection");
        }

        public async Task<int> CreateProject(Project project)
        {
            if (project != null)
            {
                try
                {
                    project.DateCreated = DateTime.Now;
                    using (var cnn = new NpgsqlConnection(connection))
                    {
                        var sql = @"INSERT INTO ""Projects"" " +
                                        @"(""Name"", ""Company"", ""Description"", ""StartDate"", ""EndDate"", ""DateCreated"", " +
                                        @"""LastUpdated"") VALUES (@Name, @Company, @Description, @StartDate, @EndDate, @DateCreated, " + @" @LastUpdated)";
                        var res = (int)await cnn.ExecuteAsync(sql, project);
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

        public async Task<int> DeleteProject(int Id)
        {
            if (Id > 0)
            {
                try
                {
                    using (var cnn = new NpgsqlConnection(connection))
                    {
                        var parameters = new DynamicParameters();
                        parameters.Add("id", Id);
                        var sql = @"DELETE FROM ""Projects"" WHERE ""Id"" = @id";
                        var project = await cnn.ExecuteAsync(sql, parameters);
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

        public async Task<Project> GetProjectById(int Id)
        {
            try
            {
                using (var cnn = new NpgsqlConnection(connection))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("id", Id);
                    var sql = @"SELECT * FROM ""Projects"" WHERE ""Id"" = @id";
                    var user = await cnn.QueryAsync<Project>(sql, parameters);
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

        public async Task<DapperQueryDTO<Project>> GetProjects(PaginationParams pagination)
        {
            try
            {
                using (var cnn = new NpgsqlConnection(connection))
                {
                    var offset = (pagination.PageNumber - 1) * pagination.PageSize;
                    var parameters = new DynamicParameters();
                    parameters.Add("Offset", offset);
                    parameters.Add("Limit", pagination.PageSize);
                    var sql = @"SELECT * FROM ""Projects"" ORDER BY ""Id"" OFFSET @Offset LIMIT @Limit ";
                    var sqlCount = @"SELECT COUNT(*) FROM ""Projects""";

                    var projects = await cnn.QueryAsync<Project>(sql, parameters);
                    var count = await cnn.QueryAsync<int>(sqlCount);
                    int countValue = count.Single();

                    return new DapperQueryDTO<Project>(projects, countValue);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<int> UpdateProject(int Id, Project project)
        {
            if (Id == project.Id)
            {
                try
                {
                    project.LastUpdated = DateTime.Now;
                    using (var cnn = new NpgsqlConnection(connection))
                    {
                        var sql = @"UPDATE ""Projects"" SET " +
                                        @"""Name"" = @Name, " +
                                        @"""Company"" = @Company, " +
                                        @"""Description"" = @Description, " +
                                        @"""StartDate"" = @StartDate, " +
                                        @"""EndDate"" = @EndDate, " +
                                        @"""DateCreated"" = @DateCreated, " +
                                        @"""LastUpdated"" = @LastUpdated " +
                                        @"WHERE (""Id"" = @Id) " +
                                        @"RETURNING *";
                        var res = (int)await cnn.ExecuteAsync(sql, project);
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
    }
}
