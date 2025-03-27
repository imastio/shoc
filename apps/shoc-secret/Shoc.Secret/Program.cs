using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Shoc.ApiCore;
using Shoc.ApiCore.Access;
using Shoc.ApiCore.Auth;
using Shoc.ApiCore.DataProtection;
using Shoc.ApiCore.Discovery;
using Shoc.ApiCore.Grpc;
using Shoc.ApiCore.ObjectAccess;
using Shoc.Secret.Config;
using Shoc.Secret.Grpc;
using Shoc.Secret.Services;

// start building web application
var builder = WebApplication.CreateBuilder(args);

// add environment variables
builder.Configuration.AddEnvironmentVariables();

// register services
builder.Services.AddDiscovery(builder.Configuration);
builder.Services.AddRepositories(builder.Configuration);
builder.Services.AddSelf(builder.Configuration);
builder.Services.AddPersistenceDataProtection();
builder.Services.AddDefaultAuthenticationMiddleware(JwtBearerDefaults.AuthenticationScheme).AddJwtAuthentication(builder.Configuration);
builder.Services.AddAccessAuthorization();
builder.Services.AddAuthenticationClient(builder.Configuration);
builder.Services.AddGrpcClients();
builder.Services.AddObjectAccessEssentials();
builder.Services.AddGrpcEssentials();
builder.Services.AddSingleton<UnifiedSecretGrpcMapper>();
builder.Services.AddSingleton<SecretProtectionProvider>();
builder.Services.AddSingleton<UserSecretProtectionProvider>();
builder.Services.AddSingleton<SecretValidationService>();
builder.Services.AddSingleton<SecretService>();
builder.Services.AddSingleton<UserSecretService>();
builder.Services.AddSingleton<WorkspaceSecretService>();
builder.Services.AddSingleton<WorkspaceUserSecretService>();
builder.Services.AddSingleton<UnifiedSecretService>();
builder.Services.AddAnyOriginCors(ApiDefaults.DEFAULT_CORS);
builder.Services.AddControllers();

// build the application
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
app.UseRouting();
app.UseAuthentication();
app.UseAccessEnrichment();
app.UseAuthorization();
app.MapControllers();
app.MapGrpcReflectionService().AllowAnonymous();
app.MapGrpcService<UnifiedSecretServiceGrpc>();
app.UseCors(ApiDefaults.DEFAULT_CORS);
app.Run();