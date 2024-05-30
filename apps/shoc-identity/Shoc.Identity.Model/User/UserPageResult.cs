using System.Collections.Generic;

namespace Shoc.Identity.Model.User
{
    /// <summary>
    /// The users page result
    /// </summary>
    public class UserPageResult
    {
        /// <summary>
        /// The result items
        /// </summary>
        public IEnumerable<UserModel> Items { get; set; }

        /// <summary>
        /// The total count 
        /// </summary>
        public long TotalCount { get; set; }
    }
}