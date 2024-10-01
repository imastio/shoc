namespace Shoc.Executor.Model.Job
{
    /// <summary>
    /// The definitions of job statuses
    /// </summary>
    public static class JobStatuses
    {
        /// <summary>
        /// The job was initialized
        /// </summary>
        public const string INIT = "init";

        /// <summary>
        /// The job is deploying
        /// </summary>
        public const string DEPLOYING = "deploying";
    }
}