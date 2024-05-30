using System;

namespace Shoc.Identity.Model.User
{
    /// <summary>
    /// The model to update user lockout
    /// </summary>
    public class UserLockoutUpdateModel
    {
        /// <summary>
        /// The lockout end time
        /// </summary>
        public DateTime? LockedUntil { get; set; }
    }
}