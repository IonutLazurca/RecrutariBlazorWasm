using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RecrutariBlazorWasm.Server.Data;
using RecrutariBlazorWasm.Server.Extensions;
using RecrutariBlazorWasm.Server.Interfaces;

namespace RecrutariBlazorWasm.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddIdentityServices(Configuration);
            services.AddApplicationServices(Configuration);
            services.AddControllersWithViews();
            services.AddRazorPages();
            services.AddScoped<IDbInitializer, DbInitializer>();
            //services.Configure<ForwardedHeadersOptions>(options =>
            //{
            //    options.KnownProxies.Add(IPAddress.Parse("192.168.1.101"));
            //});

            //services.AddSwaggerGen(c =>
            //{
            //    c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebApiTest", Version = "v1" });
            //});

            services.AddCors(o => o.AddPolicy("GoRecruit", builder =>
            {
                //builder.WithOrigins("https://royalbaby.ro:5001", "https://www.royalbaby.ro:5001", "https://localhost:5001", "http://localhost:4000", "https://localhost:4001").AllowAnyMethod().AllowAnyHeader();
                builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
            }));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IDbInitializer dbInitializer)
        {
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebAssemblyDebugging();
                //app.UseSwagger();
                //app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApiTest v1"));
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
                //app.UseSwagger();
                //app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApiTest v1"));
            }



            app.UseHttpsRedirection();
            app.UseCors("GoRecruit");

            //////
            //app.Use(async (context, next) =>
            //{
            //    context.Response.Headers.Add("X-XSS-Protection", "1; mode=block ");
            //    context.Response.Headers.Add("Content-Security-Policy", "default-src 'self';");
            //    context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
            //    await next.Invoke();
            //});
            //////
            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            dbInitializer.InitializeData();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapFallbackToFile("index.html");
            });
        }
    }
}
