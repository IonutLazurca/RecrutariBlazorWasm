using Dapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Npgsql;
using RecrutariBlazorWasm.Server.Interfaces;
using RecrutariBlazorWasm.Shared;
using RecrutariBlazorWasm.Shared.Entities;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace RecrutariBlazorWasm.Server.Data
{
    public class DbInitializer : IDbInitializer
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<Recruiter> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public string connection { get; set; }
        public DbInitializer(ApplicationDbContext db, IConfiguration configuration, UserManager<Recruiter> userManager, RoleManager<IdentityRole> roleManager)
        {
            connection = configuration.GetConnectionString("PostgresqlConnection");
            this._db = db;
            this._userManager = userManager;
            this._roleManager = roleManager;
        }

        public async Task InitializeData()
        {
            //try
            //{
            //    if(_db.Database.GetPendingMigrations().Count() > 0)
            //    {
            //        _db.Database.Migrate();
            //    }
            //}
            //catch (System.Exception)
            //{
            //    throw;
            //}
            if (!_db.Roles.Any(x => x.Name == SD.Role_Admin))
            {
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Admin)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Recruiter)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Applicant)).GetAwaiter().GetResult();
            };

            if (!await CheckExistingData("Applicants"))
            {
                using (StreamReader r = new StreamReader("C:\\Users\\Ionut\\source\\repos\\RecrutariBlazorWasm\\Server\\Data\\Initializer\\applicants.json"))
                {
                    string json = r.ReadToEnd();
                    List<Applicant> applicants = JsonConvert.DeserializeObject<List<Applicant>>(json);
                    using (var cnn = new NpgsqlConnection(connection))
                    {
                        var sql = @"INSERT INTO ""Applicants"" (""FirstName"", ""LastName"", ""Gender"", ""DateOfBirth"", ""Nationality"", ""HomePhone"", ""MobilePhone"", ""Email"", ""City"", ""Country"") VALUES (@firstname, @lastname,@gender, @dateofbirth, @nationality, @homephone, @mobilephone, @email, @city, @country)";
                        var populateData = await cnn.ExecuteAsync(sql, applicants);
                    }
                }
            }

            if (!await CheckExistingData("Projects"))
            {
                var json = File.ReadAllText("C:\\Users\\Ionut\\source\\repos\\RecrutariBlazorWasm\\Server\\Data\\Initializer\\projects.json");
                List<Project> projects = JsonConvert.DeserializeObject<List<Project>>(json);
                using (var cnn = new NpgsqlConnection(connection))
                {
                    var sql = @"INSERT INTO ""Projects"" (""Name"", ""Company"", ""StartDate"", ""EndDate"") VALUES (@Name, @Company, @StartDate, @EndDate)";
                    var populateData = await cnn.ExecuteAsync(sql, projects);
                }
            }

            if (!await CheckExistingData("Holidays"))
            {
                var json = File.ReadAllText("C:\\Users\\Ionut\\source\\repos\\RecrutariBlazorWasm\\Server\\Data\\Holidays\\holidays.json");
                var holidays = JsonConvert.DeserializeObject<List<Holiday>>(json);
                using (var cnn = new NpgsqlConnection(connection))
                {
                    var sql = @"INSERT INTO ""Holidays"" (""Title"", ""Date"") VALUES (@Title, @Date)";
                    var populateData = await cnn.ExecuteAsync(sql, holidays);
                }
            }

            //if (!await CheckExistingData("ApplicantProject"))
            //{
            //    var data = File.ReadAllLines("C:\\Users\\Ionut\\Desktop\\Recrutari\\applicantproject.csv").Skip(1).Select(v => ApplicantProject.FromCSV(v)).ToList();

            //    using (var cnn = new NpgsqlConnection(connection))
            //    {
            //        var sql = @"INSERT INTO ""ApplicantProject"" (""EnrollDate"", ""CancelDate"", ""ApplicantId"") VALUES (@EnrollDate, @CancelDate, @ApplicantId)";
            //        var populateData = await cnn.ExecuteAsync(sql, data);
            //    }
            //}
        }

        public async Task<bool> CheckExistingData(string database)
        {
            var sql = $@"SELECT COUNT(*) FROM ""{database}""";
            using (var cnn = new NpgsqlConnection(connection))
            {
                var res = await cnn.QueryAsync<int>(sql);
                var result = res.Single();
                if (result > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }


    }
}
