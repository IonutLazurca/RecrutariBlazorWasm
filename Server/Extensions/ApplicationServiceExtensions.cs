using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;
using RecrutariBlazorWasm.Server.Data;
using RecrutariBlazorWasm.Server.Data.Repository;
using RecrutariBlazorWasm.Server.Interfaces;

namespace RecrutariBlazorWasm.Server.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(config.GetConnectionString("PostgresqlConnection")));

            services.AddScoped<IApplicantRepository, ApplicantRepository>();
            services.AddScoped<IProjectRepository, ProjectRepository>();
            services.AddScoped<IAplicantProjectRepository, AplicantProjectRepository>();

            services.AddControllers().AddJsonOptions(opt => opt.JsonSerializerOptions.PropertyNamingPolicy = null)
                .AddNewtonsoftJson(opt =>
                {
                    opt.SerializerSettings.ContractResolver = new DefaultContractResolver();
                    opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                });

            return services;
        }
    }
}
