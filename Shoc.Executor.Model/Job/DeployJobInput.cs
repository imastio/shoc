namespace Shoc.Executor.Model.Job
{
    /// <summary>
    /// The job deploy input
    /// </summary>
    public class DeployJobInput
    {
        /// <summary>
        /// The job id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The workers count
        /// </summary>
        public int Workers { get; set; }
    }
}