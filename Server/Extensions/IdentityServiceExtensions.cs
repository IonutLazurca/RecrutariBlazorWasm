using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using RecrutariBlazorWasm.Server.Data;
using RecrutariBlazorWasm.Server.Interfaces;
using RecrutariBlazorWasm.Server.Services;
using RecrutariBlazorWasm.Shared.Entities;
using System;
using System.Text;

namespace RecrutariBlazorWasm.Server.Extensions
{
    public static class IdentityServiceExtensions
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddIdentity<Recruiter, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

            services.AddScoped<ITokenService, TokenService>();

            var appSettingsSection = config.GetSection("ApiSettings");
            var apiSettings = appSettingsSection.Get<ApiSettings>();

            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(apiSettings.SecretKey)),
                        ValidateAudience = true,
                        ValidateIssuer = true,
                        ValidAudience = apiSettings.ValidAudience,
                        ValidIssuer = apiSettings.ValidIssuer,
                        ClockSkew = TimeSpan.Zero
                    };
                }
                );

            return services;
        }
    }
}
