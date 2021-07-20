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

namespace RecrutariBlazorWasm.Server.Data.GenericRepo
{
    public class AppRepository : IRepository<Applicant>
    {
        public string connection { get; set; }

        public AppRepository(IConfiguration configuration)
        {
            connection = configuration.GetConnectionString("PostgresqlConnection");
        }

        public async Task<int> Create(Applicant applicant)
        {
            if (applicant != null)
            {
                try
                {
                    using (var cnn = new NpgsqlConnection(connection))
                    {
                        var sql = @"INSERT INTO ""Applicants"" " +
                                        @"(""FirstName"", ""LastName"", ""Gender"", ""DateOfBirth"", ""Nationality"", ""HomePhone"", " +
                                        @"""MobilePhone"", ""Email"", ""City"", ""Country"") VALUES(" +
                                        "@FirstName, @LastName, @Gender, @DateOfBirth, @Nationality, @HomePhone, @MobilePhone, @Email, " + "@City, @Country)";
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

        public async Task<int> Delete(int Id)
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

        public async Task<Applicant> GetById(int Id)
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

        public async Task<DapperQueryDTO<Applicant>> Get(PaginationParams pagination)
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

        public async Task<int> Update(int Id, Applicant applicant)
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
                                        @"""Country"" = @Country " +
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
    }
}
