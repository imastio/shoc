using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Shoc.ApiCore;
using Shoc.ApiCore.Auth;
using Shoc.ApiCore.AuthClient;
using Shoc.ApiCore.DataProtection;
using Shoc.ApiCore.Discovery;
using Shoc.Builder.Client;
using Shoc.Executor.Config;
using Shoc.Executor.Services;
using Shoc.Executor.Services.Interfaces;

namespace Shoc.Executor
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
            services.AddSelf(this.Configuration);
            services.AddExecutor(this.Configuration);
            services.AddPersistenceDataProtection();
            services.AddDiscovery(this.Configuration);
            services.AddRepositories(this.Configuration);
            services.AddAuthenticationMiddleware(this.Configuration);

            services.AddClient((discovery, settings) => new BuilderClient(settings.Application, discovery));
            services.AddAuthenticationClient(this.Configuration);

            services.AddSingleton<JobService>();
            services.AddSingleton<KubernetesClusterService>();
            services.AddSingleton<IDeploymentProvider, DeploymentProvider>();

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Shoc.Executor", Version = "v1" });
            });
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
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Shoc.Executor v1"));
            }

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}
