using System.Threading.Tasks;
using Shoc.Core;
using Shoc.Identity.Model.Application;
using Shoc.Identity.Provider.Data;

namespace Shoc.Identity.Services;

/// <summary>
/// The base service for the application
/// </summary>
public class ApplicationServiceBase
{
    /// <summary>
    /// The application repository
    /// </summary>
    protected readonly IApplicationRepository applicationRepository;

    /// <summary>
    /// Creates new instance of application service
    /// </summary>
    /// <param name="applicationRepository">The application repository</param>
    protected ApplicationServiceBase(IApplicationRepository applicationRepository)
    {
        this.applicationRepository = applicationRepository;
    }

    /// <summary>
    /// Require the object with given identifier to exist
    /// </summary>
    /// <param name="id">The id of the object</param>
    /// <returns></returns>
    protected async Task<ApplicationModel> RequireById(string id)
    {
        // try load the object
        var existing = await this.applicationRepository.GetById(id);

        // make sure object exists
        if (existing == null)
        {
            throw ErrorDefinition.NotFound().AsException();
        }

        return existing;
    }
}