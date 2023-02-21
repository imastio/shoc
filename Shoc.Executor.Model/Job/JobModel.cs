using System;

namespace Shoc.Executor.Model.Job
{
    /// <summary>
    /// The job data representation
    /// </summary>
    public class JobModel
    {
        /// <summary>
        /// The job id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The project id
        /// </summary>
        public string ProjectId { get; set; }

        /// <summary>
        /// The package id
        /// </summary>
        public string PackageId { get; set; }

        /// <summary>
        /// The owner id
        /// </summary>
        public string OwnerId { get; set; }

        /// <summary>
        /// The state of job
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// The run specification for job
        /// </summary>
        public string RunSpec { get; set; }

        /// <summary>
        /// The progress indicator
        /// </summary>
        public int Progress { get; set; }

        /// <summary>
        /// The progress message
        /// </summary>
        public string ProgressMessage { get; set; }

        /// <summary>
        /// The job creation time
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        /// The job update time
        /// </summary>
        public DateTime Updated { get; set; }

        /// <summary>
        /// The run info
        /// </summary>
        public string RunInfo { get; set; }
    }
}
