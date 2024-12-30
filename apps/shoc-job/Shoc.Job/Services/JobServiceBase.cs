using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
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
    
    /// <summary>
    /// Converts the object to json string with camel case convention
    /// </summary>
    /// <param name="input">The input to serialize</param>
    /// <typeparam name="T">The type to serialize</typeparam>
    /// <returns></returns>
    protected static string ToJsonString<T>(T input)
    {
        return JsonConvert.SerializeObject(input, new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy
                {
                    ProcessDictionaryKeys = false
                }
            }
        });
    }

    /// <summary>
    /// Deserialize given json into an object
    /// </summary>
    /// <param name="json">The json string</param>
    /// <typeparam name="T">The type to deserialize to</typeparam>
    /// <returns></returns>
    protected static T FromJsonString<T>(string json)
    {
        return JsonConvert.DeserializeObject<T>(json);
    }
}