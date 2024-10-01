namespace Shoc.Executor.Model.Job
{
    /// <summary>
    /// The job status update structure
    /// </summary>
    public class JobStatusModel
    {
        /// <summary>
        /// The job id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The status of job
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// The job progress
        /// </summary>
        public int Progress { get; set; }

        /// <summary>
        /// The progress message
        /// </summary>
        public string ProgressMessage { get; set; }
    }
}