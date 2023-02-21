using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Diagnostics;
using System.Net.Http;
using System.Net;
using Yarp.ReverseProxy.Forwarder;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Shoc.ApiCore;
using Shoc.ApiCore.Discovery;
using Shoc.Webgtw.Gateway;

namespace Shoc.Webgtw
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
            services.AddHttpForwarder();
            services.AddSelf(this.Configuration);
            services.AddAnyOriginCors(ApiDefaults.DEFAULT_CORS);
            services.AddControllers();

            services.Configure<FormOptions>(options =>
            {
                options.MultipartBodyLengthLimit = long.MaxValue;
                options.BufferBodyLengthLimit = long.MaxValue;
                options.MultipartBoundaryLengthLimit = int.MaxValue;
            });

            services.Configure<KestrelServerOptions>(options =>
            {
                options.Limits.MaxRequestBodySize = int.MaxValue;
            });

            // add proxy essentials
            services.AddSingleton<RequestTransformer>();
        }

        /// <summary>
        /// Configure the HTTP request pipeline
        /// </summary>
        /// <param name="app">The app</param>
        /// <param name="env">The environment</param>
        /// <param name="forwarder">The HTTP forwarder</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHttpForwarder forwarder)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseCors(ApiDefaults.DEFAULT_CORS);

            // customize endpoint forwarding
            app.UseEndpoints(endpoints =>
            {
                endpoints.Map("/{service:regex(^shoc-.*)}/{**api}", async httpContext=>
                {
                    // get transformer service from DI
                    var transformer = app.ApplicationServices.GetRequiredService<RequestTransformer>();

                    // configure our own HttpMessageInvoker for outbound calls for proxy operations
                    var httpInvoker = new HttpMessageInvoker(new SocketsHttpHandler
                    {
                        UseProxy = false,
                        AllowAutoRedirect = false,
                        AutomaticDecompression = DecompressionMethods.None,
                        UseCookies = false,
                        ActivityHeadersPropagator = new ReverseProxyPropagator(DistributedContextPropagator.Current)
                    });

                    // try forward
                    await forwarder.SendAsync(httpContext, httpContext.Request.GetDisplayUrl(), httpInvoker, ForwarderRequestConfig.Empty, transformer);
                });

                endpoints.MapControllers();
            });

        }
    }
}
