using Duende.IdentityServer;
using Duende.IdentityServer.Services;
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
using Shoc.ApiCore.Intl;
using Shoc.ApiCore.Mailing;
using Shoc.ApiCore.RazorEngine;
using Shoc.Identity.Config;
using Shoc.Identity.Config.Oidc;
using Shoc.Identity.Grpc;
using Shoc.Identity.Provider.Config;
using Shoc.Identity.Provider.Services;
using Shoc.Identity.Services;

// start building web application
var builder = WebApplication.CreateBuilder(args);

// add environment variables
builder.Configuration.AddEnvironmentVariables();

// register services
builder.Services.AddDiscovery(builder.Configuration);
builder.Services.AddRepositories(builder.Configuration);
builder.Services.AddSelf(builder.Configuration);
builder.Services.AddSignOnEssentials(builder.Configuration);
builder.Services.AddIdentityEssentials(builder.Configuration);
builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultScheme = IdentityServerConstants.DefaultCookieAuthenticationScheme;
        options.DefaultAuthenticateScheme = IdentityServerConstants.DefaultCookieAuthenticationScheme;
        options.DefaultChallengeScheme = IdentityServerConstants.DefaultCookieAuthenticationScheme;
        options.DefaultSignInScheme = IdentityServerConstants.DefaultCookieAuthenticationScheme;
    })
    .AddJwtAuthentication(builder.Configuration)
    .AddExternalOidcEssentials();
builder.Services.AddAuthorization();
builder.Services.AddAccessAuthorization();
builder.Services.AddPersistenceDataProtection();
builder.Services.AddMailing(builder.Configuration);
builder.Services.AddIntlEssentials(builder.Configuration);
builder.Services.AddRazorEngine();
builder.Services.AddControllersWithViews();
builder.Services.AddControllers();
builder.Services.AddAnyOriginCors(ApiDefaults.DEFAULT_CORS);
builder.Services.AddForwardingConfiguration();
builder.Services.AddResponseCompression(options => options.EnableForHttps = true);
builder.Services.AddSameSiteCookiePolicy();
builder.Services.AddGrpcEssentials();
builder.Services.AddSingleton<ICorsPolicyService, CorsPolicyService>();
builder.Services.AddSingleton<SigninHandler>();
builder.Services.AddSingleton<UserService>();
builder.Services.AddSingleton<UserAccessService>();
builder.Services.AddSingleton<UserPasswordService>();
builder.Services.AddSingleton<CurrentUserService>();
builder.Services.AddSingleton<CurrentSessionService>();
builder.Services.AddSingleton<CredentialEvaluator>();
builder.Services.AddSingleton<UserGroupService>();
builder.Services.AddSingleton<UserGroupMembersService>();
builder.Services.AddSingleton<UserGroupAccessService>();
builder.Services.AddSingleton<PrivilegeService>();
builder.Services.AddSingleton<PrivilegeAccessService>();
builder.Services.AddSingleton<RoleService>();
builder.Services.AddSingleton<RolePrivilegeService>();
builder.Services.AddSingleton<RoleMembersService>();
builder.Services.AddSingleton<ApplicationService>();
builder.Services.AddSingleton<ApplicationSecretService>();
builder.Services.AddSingleton<ApplicationUriService>();
builder.Services.AddSingleton<ApplicationClaimService>();
builder.Services.AddSingleton<IdentityProviderProtectionProvider>();
builder.Services.AddSingleton<OidcProviderService>();
builder.Services.AddSingleton<OidcProviderDomainService>();
builder.Services.AddScoped<PasswordRecoveryService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<OtpService>();
builder.Services.AddScoped<ConfirmationService>();

// build the application
var app = builder.Build();

app.UseMiddleware<NonApiSpaMiddleware>();
app.UseDefaultFiles();
app.UseStaticFiles();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseForwardedHeaders();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseForwardedHeaders();
    app.UseHsts();
}

app.UseCookiePolicy();
app.UseRouting();
app.UseIdentityServer();
app.UseAuthentication();
app.UseAccessEnrichment();
app.UseAuthorization();
app.MapControllers();
app.MapGrpcReflectionService().AllowAnonymous();
app.MapGrpcService<UserServiceGrpc>();
app.UseCors(ApiDefaults.DEFAULT_CORS);

app.UseResponseCompression();
app.Run();