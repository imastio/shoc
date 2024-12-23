namespace Shoc.Job.Model.Job;

/// <summary>
/// The job run manifest specification model
/// </summary>
public class JobRunManifestSpecModel
{
    /// <summary>
    /// The MPI specification
    /// </summary>
    public JobRunManifestSpecMpiModel Mpi { get; set; }
}