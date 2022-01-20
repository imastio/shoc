using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Shoc.ApiCore.Discovery;
using Shoc.Builder.Config;
using Shoc.Builder.Services;

namespace Shoc.Builder
{
    /// <summary>
    /// The startup application
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// The configuration
        /// </summary>
        private IConfiguration Configuration { get; }

        /// <summary>
        /// Creates new instance of startup
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        /// <summary>
        /// Configure services
        /// </summary>
        /// <param name="services">The services to configure</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDiscovery(this.Configuration);
            services.AddEngineClient(this.Configuration);
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Shoc.Builder", Version = "v1" });
            });

            services.AddSingleton<EngineService>();
        }

        /// <summary>
        /// Configure the HTTP request pipeline
        /// </summary>
        /// <param name="app">The app</param>
        /// <param name="env">The environment</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Shoc.Builder v1"));
            }

            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}
