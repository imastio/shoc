using System.Collections.Generic;

namespace Shoc.Identity.Model
{
    /// <summary>
    /// The predefined roles in the system
    /// </summary>
    public class Roles
    {
        /// <summary>
        /// The root user role
        /// </summary>
        public const string ROOT = "root";

        /// <summary>
        /// The role of administrator
        /// </summary>
        public const string ADMIN = "admin";

        /// <summary>
        /// The role of regular user
        /// </summary>
        public const string USER = "user";

        /// <summary>
        /// The set of administrative roles
        /// </summary>
        public static readonly ISet<string> ADMINS = new HashSet<string> {ROOT, ADMIN};
    }
}