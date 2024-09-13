using System.Diagnostics;
using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Shoc.ApiCore;
using Shoc.ApiCore.Discovery;
using Shoc.Webgtw.Gateway;
using Yarp.ReverseProxy.Forwarder;

// start building web application
var builder = WebApplication.CreateBuilder(args);

// add environment variables
builder.Configuration.AddEnvironmentVariables();

// register services
builder.Services.AddDiscovery(builder.Configuration);
builder.Services.AddHttpForwarder();
builder.Services.AddSelf(builder.Configuration);
builder.Services.AddAnyOriginCors(ApiDefaults.DEFAULT_CORS);
builder.Services.AddControllers();

builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = long.MaxValue;
    options.BufferBodyLengthLimit = long.MaxValue;
    options.MultipartBoundaryLengthLimit = int.MaxValue;
});

builder.Services.Configure<KestrelServerOptions>(options =>
{
    options.Limits.MaxRequestBodySize = int.MaxValue;
});
            
// add proxy essentials
builder.Services.AddSingleton<RequestTransformer>();

// build the application
var app = builder.Build();

// build pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseRouting();
app.UseCors(ApiDefaults.DEFAULT_CORS);

app.Map("/{service:regex(^shoc-.*)}/{**api}", async httpContext=>
{
    // get transformer service from DI
    var forwarder = app.Services.GetRequiredService<IHttpForwarder>();
    var transformer = app.Services.GetRequiredService<RequestTransformer>();

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

app.MapControllers();
app.Run();