namespace Shoc.Job.Model.Job;

/// <summary>
/// The job run manifest model
/// </summary>
public class JobRunManifestModel
{
    /// <summary>
    /// The git repo to associate with
    /// </summary>
    public string GitRepoId { get; set; }
    
    /// <summary>
    /// The label ids
    /// </summary>
    public string[] LabelIds { get; set; }
    
    /// <summary>
    /// The cluster id
    /// </summary>
    public string ClusterId { get; set; }
    
    /// <summary>
    /// The package id
    /// </summary>
    public string PackageId { get; set; }
    
    /// <summary>
    /// The given arguments
    /// </summary>
    public string[] Args { get; set; }
    
    /// <summary>
    /// The array configuration of the job
    /// </summary>
    public JobRunManifestArrayModel Array { get; set; }
    
    /// <summary>
    /// The environment configuration of the job
    /// </summary>
    public JobRunManifestEnvModel Env { get; set; }
    
    /// <summary>
    /// The resources configuration of the job
    /// </summary>
    public JobRunManifestResourcesModel Resources { get; set; }
    
    /// <summary>
    /// The specification of the job
    /// </summary>
    public JobRunManifestSpecModel Spec { get; set; }
}