namespace Shoc.Engine.Model
{
    /// <summary>
    /// The registry definition
    /// </summary>
    public class RegistryDefinition
    {
        /// <summary>
        /// The registry identity
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The registry Uri
        /// </summary>
        public string RegistryUri { get; set; }

        /// <summary>
        /// The repository
        /// </summary>
        public string Repository { get; set; }

        /// <summary>
        /// The registry email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// The registry username
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// The registry password
        /// </summary>
        public string Password { get; set; }
    }
}