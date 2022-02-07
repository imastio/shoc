using System;

namespace Shoc.Builder.Model
{
    /// <summary>
    /// The docker registry model
    /// </summary>
    public class DockerRegistry
    {
        /// <summary>
        /// The registry id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The registry uri value
        /// </summary>
        public string RegistryUri { get; set; }

        /// <summary>
        /// The registry email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// The username value
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// The password in encrypted form
        /// </summary>
        public byte[] EncryptedPassword { get; set; }

        /// <summary>
        /// The creation time
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        /// The update time
        /// </summary>
        public DateTime Updated { get; set; }
    }
}