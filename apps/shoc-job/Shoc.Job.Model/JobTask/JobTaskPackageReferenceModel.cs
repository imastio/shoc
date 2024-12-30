namespace Shoc.Job.Model.JobTask;

/// <summary>
/// The package reference of job task
/// </summary>
public class JobTaskPackageReferenceModel
{
    /// <summary>
    /// The image url
    /// </summary>
    public string Image { get; set; }
    
    /// <summary>
    /// The username to pull the image
    /// </summary>
    public string PullUsername { get; set; }
    
    /// <summary>
    /// The plaintext password to pull the image
    /// </summary>
    public string PullPasswordPlain { get; set; }
}