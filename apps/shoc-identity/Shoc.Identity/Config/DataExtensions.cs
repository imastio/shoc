using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shoc.Access.Data;
using Shoc.Access.Data.Sql;
using Shoc.ApiCore;
using Shoc.Data.Sql;
using Shoc.DataProtection;
using Shoc.DataProtection.Sql;
using Shoc.Identity.Provider.Data;
using Shoc.Identity.Provider.Data.Mysql;
using Shoc.Mailing;
using Shoc.Mailing.Sql;

namespace Shoc.Identity.Config;

/// <summary>
/// The data configuration of system
/// </summary>
public static class DataConfiguration
{
    /// <summary>
    /// Configure data environment
    /// </summary>
    /// <param name="services">The services collection</param>
    /// <param name="configuration">The configuration instance</param>
    public static IServiceCollection AddRepositories(this IServiceCollection services, IConfiguration configuration)
    {
        // get settings from configuration
        var dataSettings = configuration.BindAs<DataSourceSettings>("DataSource");

        // register settings for future use
        services.AddSingleton(dataSettings);

        // get the set of sources
        var sources = IdentityOperations.GetSources();

        // build and init new data operations instance
        var dataOps = OperationsInitializer.Init(dataSettings, sources.ToArray());

        // add data operations
        services.AddSingleton(dataOps);

        // add domain repositories
        services.AddSingleton<IAccessGrantRepository, AccessGrantRepository>();
        services.AddSingleton<ISigningKeyRepository, SigningKeyRepository>();
        services.AddSingleton<IApplicationRepository, ApplicationRepository>();
        services.AddSingleton<IApplicationSecretRepository, ApplicationSecretRepository>();
        services.AddSingleton<IApplicationUriRepository, ApplicationUriRepository>();
        services.AddSingleton<IApplicationClaimRepository, ApplicationClaimRepository>();
        services.AddSingleton<IUserRepository, UserRepository>();
        services.AddSingleton<IUserAccessRepository, UserAccessRepository>();
        services.AddSingleton<IUserInternalRepository, UserInternalRepository>();
        services.AddSingleton<IOtpRepository, OtpRepository>();
        services.AddSingleton<IConfirmationCodeRepository, ConfirmationCodeRepository>();
        services.AddSingleton<ISigninHistoryRepository, SigninHistoryRepository>();
        services.AddSingleton<IUserGroupRepository, UserGroupRepository>();
        services.AddSingleton<IUserGroupAccessRepository, UserGroupAccessRepository>();
        services.AddSingleton<IUserGroupMemberRepository, UserGroupMemberRepository>();
        services.AddSingleton<IProtectionKeyRepository, ProtectionKeyRepository>();
        services.AddSingleton<IMailingProfileRepository, MailingProfileRepository>();
        services.AddSingleton<IAccessRepository, AccessRepository>();
        services.AddSingleton<IPrivilegeRepository, PrivilegeRepository>();
        services.AddSingleton<IPrivilegeAccessRepository, PrivilegeAccessRepository>();
        services.AddSingleton<IRoleRepository, RoleRepository>();
        services.AddSingleton<IRolePrivilegeRepository, RolePrivilegeRepository>();
        services.AddSingleton<IRoleMemberRepository, RoleMemberRepository>();

        // chain services
        return services;
    }
}