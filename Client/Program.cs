using Blazored.LocalStorage;
using MatBlazor;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor.Services;
using RecrutariBlazorWasm.Client.Interfaces;
using RecrutariBlazorWasm.Client.Repository;
using RecrutariBlazorWasm.Client.Service;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace RecrutariBlazorWasm.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            //builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.Configuration.GetValue<string>("BaseAPIUrl")) });
            ConfigureServices(builder.Services);
            await builder.Build().RunAsync();
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IApplicantRepository, ApplicantRepository>();
            services.AddScoped<IProjectRepository, ProjectRepository>();
            services.AddScoped<IApplicantProjectRepository, ApplicantProjectRepository>();
            services.AddScoped<IHttpService, HttpService>();
            services.AddMatBlazor();
            services.AddMudServices();
            services.AddBlazoredLocalStorage();
        }
    }
}
