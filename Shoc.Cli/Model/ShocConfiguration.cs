using System.Collections.Generic;

namespace Shoc.Cli.Model
{
    /// <summary>
    /// The shoc configuration
    /// </summary>
    public class ShocConfiguration
    {
        /// <summary>
        /// The default profile
        /// </summary>
        public string DefaultProfile { get; set; }

        /// <summary>
        /// The definitions
        /// </summary>
        public List<ShocProfile> Profiles { get; set; }
    }
}