namespace Shoc.Job.Model.Job;

/// <summary>
/// The job run manifest array model
/// </summary>
public class JobRunManifestArrayModel
{
    /// <summary>
    /// The number of replicas
    /// </summary>
    public long Replicas { get; set; }
    
    /// <summary>
    /// The indexer variable for enumeration
    /// </summary>
    public string Indexer { get; set; }
}