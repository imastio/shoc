using Shoc.Job.Data;

namespace Shoc.Job.Services;

/// <summary>
/// The base service for job operations
/// </summary>
public abstract class JobServiceBase
{
    /// <summary>
    /// The job repository
    /// </summary>
    protected readonly IJobRepository jobRepository;

    /// <summary>
    /// The job validation service
    /// </summary>
    protected readonly JobValidationService validationService;

    /// <summary>
    /// The protection provider
    /// </summary>
    protected readonly JobProtectionProvider jobProtectionProvider;

    /// <summary>
    /// The job base service
    /// </summary>
    /// <param name="jobRepository">The job repository</param>
    /// <param name="validationService">The validation service</param>
    /// <param name="jobProtectionProvider">The protection provider</param>
    protected JobServiceBase(IJobRepository jobRepository, JobValidationService validationService, JobProtectionProvider jobProtectionProvider)
    {
        this.jobRepository = jobRepository;
        this.validationService = validationService;
        this.jobProtectionProvider = jobProtectionProvider;
    }
}