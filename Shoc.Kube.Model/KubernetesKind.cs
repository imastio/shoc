namespace Shoc.Kube.Model
{
    /// <summary>
    /// The known kinds of kubernetes
    /// </summary>
    public static class KubernetesKind
    {
        /// <summary>
        /// The namespace known kind
        /// </summary>
        public const string NAMESPACE = "Namespace";

        /// <summary>
        /// The secret known kind
        /// </summary>
        public const string SECRET = "Secret";

        /// <summary>
        /// The job known kind
        /// </summary>
        public const string JOB = "Job";
    }
}