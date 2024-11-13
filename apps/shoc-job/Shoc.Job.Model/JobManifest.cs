namespace Shoc.Job.Model;

/// <summary>
/// The job manifest definition
/// </summary>
public class JobManifest
{
    /// <summary>
    /// The job kind
    /// </summary>
    public string Kind { get; set; }
    
    /// <summary>
    /// The cluster identification 
    /// </summary>
    public string Cluster { get; set; }
}