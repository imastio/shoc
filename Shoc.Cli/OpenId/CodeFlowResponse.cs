namespace Shoc.Cli.OpenId
{
    /// <summary>
    /// The response of code flow worker
    /// </summary>
    public class CodeFlowResponse
    {
        /// <summary>
        /// Indicates if timed out
        /// </summary>
        public bool Timeout { get; set; }

        /// <summary>
        /// Indicates the success
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// The response string if available
        /// </summary>
        public string ResponseString { get; set; }

        /// <summary>
        /// The error message
        /// </summary>
        public string Error { get; set; }
    }
}