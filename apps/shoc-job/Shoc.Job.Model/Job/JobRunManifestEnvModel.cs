using System.Collections.Generic;

namespace Shoc.Job.Model.Job;

/// <summary>
/// The job run manifest environment model
/// </summary>
public class JobRunManifestEnvModel
{
    /// <summary>
    /// The names of secrets to inject
    /// </summary>
    public string[] Use { get; set; }
    
    /// <summary>
    /// The variables and values to override the secrets
    /// </summary>
    public Dictionary<string, string> Override { get; set; }
}