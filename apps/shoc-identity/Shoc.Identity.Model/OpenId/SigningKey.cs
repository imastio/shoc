using System;

namespace Shoc.Identity.Model
{
    /// <summary>
    /// The signing key object
    /// </summary>
    public class SigningKey
    {
        /// <summary>
        /// The id of signing key
        /// </summary>
        public string Id { get; set; }
        
        /// <summary>
        /// The algorithm of signing
        /// </summary>
        public string Algorithm { get; set; }
        
        /// <summary>
        /// The key material data
        /// </summary>
        public string Data { get; set; }
        
        /// <summary>
        /// The data protected indicator
        /// </summary>
        public bool DataProtected { get; set; }
        
        /// <summary>
        /// Indicates if key is X509 certificate
        /// </summary>
        public bool IsX509Certificate { get; set; }
        
        /// <summary>
        /// The use of key
        /// </summary>
        public string Use { get; set; }
        
        /// <summary>
        /// The key version
        /// </summary>
        public int Version { get; set; }
        
        /// <summary>
        /// The time of creation
        /// </summary>
        public DateTime Created { get; set; }
    }
}