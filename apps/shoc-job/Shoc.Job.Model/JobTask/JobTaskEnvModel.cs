using System.Collections.Generic;

namespace Shoc.Job.Model.JobTask;

/// <summary>
/// The definition and values for all referenced environment values
/// </summary>
public class JobTaskEnvModel
{
    /// <summary>
    /// The plain environment variable key values
    /// </summary>
    public Dictionary<string, string> Plain { get; set; }
    
    /// <summary>
    /// The encrypted environment variable key values
    /// </summary>
    public Dictionary<string, string> Encrypted { get; set; }
}