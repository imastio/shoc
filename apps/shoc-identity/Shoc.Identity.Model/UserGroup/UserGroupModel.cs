using System;

namespace Shoc.Identity.Model.UserGroup
{
    /// <summary>
    /// The user group model
    /// </summary>
    public class UserGroupModel
    {
        /// <summary>
        /// The user group id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The user group name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The group creation time
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        /// The group update time
        /// </summary>
        public DateTime Updated { get; set; }
    }
}