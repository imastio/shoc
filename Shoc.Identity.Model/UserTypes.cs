using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Shoc.Identity.Model
{
    /// <summary>
    /// The predefined roles in the system
    /// </summary>
    public class UserTypes
    {
        /// <summary>
        /// The root user type
        /// </summary>
        public const string ROOT = "root";

        /// <summary>
        /// The type of administrator
        /// </summary>
        public const string ADMIN = "admin";

        /// <summary>
        /// The type of internal user
        /// </summary>
        public const string USER = "user";

        /// <summary>
        /// The set of escalated user types
        /// </summary>
        public static readonly ISet<string> ESCALATED = new HashSet<string>
        {
            ROOT, ADMIN
        };

        /// <summary>
        /// Get and initialize all the constants
        /// </summary>
        public static readonly ISet<string> ALL = GetAll();

        /// <summary>
        /// Gets all the constant values
        /// </summary>
        /// <returns></returns>
        private static ISet<string> GetAll()
        {
            return typeof(UserTypes)
                .GetFields(BindingFlags.Public | BindingFlags.Static)
                .Where(f => !f.IsInitOnly && f.IsLiteral && f.FieldType == typeof(string))
                .Select(f => f.GetRawConstantValue() as string)
                .ToHashSet();
        }

    }
}