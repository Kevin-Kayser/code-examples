using System.Collections.Generic;
using code_examples.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace code_examples
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddCors(options =>
            {
                // Cors site list can be set via environmental variables. For now we're just using explicit values.
                var corsSiteList = new List<string>()
                {
                    "https://localhost:3276", "http://localhost:3276"
                };


                options.AddPolicy(name: "CorsPolicy",
                    builder =>
                    {
                        builder.WithOrigins(corsSiteList.ToArray())
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                    });
            });

            // Dependency injection settings and services.
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
            services.AddScoped<IFileSystemService, FileSystemService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            // userCors needed between UseRouting and UseEndpoints
            app.UseCors();

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}