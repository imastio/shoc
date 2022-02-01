using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Shoc.Cli.Model;
using Shoc.Cli.Utility;
using Shoc.Core;

namespace Shoc.Cli.Services
{
    /// <summary>
    /// The configuration service
    /// </summary>
    public class ConfigurationService
    {
        /// <summary>
        /// Gets or creates the shoc configuration
        /// </summary>
        /// <returns></returns>
        public async Task<ShocConfiguration> GetOrCreateConfiguration()
        {
            // get the path to shoc config file
            var path = GetShocConfigFilePath();

            // if no config file created yet
            if (!File.Exists(path))
            {
                // write a minimal default configuration
                await File.WriteAllTextAsync(path, Yml.Serialize(new ShocConfiguration
                {
                    DefaultProfile = "default",
                    Profiles = new List<ShocProfile> {new() {Name = "default"}}
                }));
            }

            // get object back
            return Yml.Deserialize<ShocConfiguration>(await File.ReadAllTextAsync(path));
        }

        /// <summary>
        /// Gets or creates the shoc configuration
        /// </summary>
        /// <returns></returns>
        public async Task<ShocConfiguration> GetConfiguration()
        {
            // get the path to shoc config file
            var path = GetShocConfigFilePath();
            
            // get object back
            return Yml.Deserialize<ShocConfiguration>(await File.ReadAllTextAsync(path));
        }

        /// <summary>
        /// Gets the profile by name
        /// </summary>
        /// <param name="name">The name of profile</param>
        /// <returns></returns>
        public async Task<ShocProfile> GetProfile(string name)
        {
            // gets the configuration
            var config = await this.GetConfiguration();

            // try find the profile
            var profile = config?.Profiles?.FirstOrDefault(profile => string.Equals(profile.Name, name ?? config.DefaultProfile, StringComparison.OrdinalIgnoreCase));

            // no profile
            if (profile == null)
            {
                throw ErrorDefinition.Validation(CliErrors.INVALID_PROFILE, "The profile is missing").AsException();
            }

            return profile;
        }

        /// <summary>
        /// Saves the given modified configuration to the system
        /// </summary>
        /// <param name="configuration">The configuration to save</param>
        /// <returns></returns>
        public async Task SaveConfiguration(ShocConfiguration configuration)
        {
            // get the path to shoc config file
            var path = GetShocConfigFilePath();

            // write all to the file 
            await File.WriteAllTextAsync(path, Yml.Serialize(configuration));
        }
        
        /// <summary>
        /// Gets the shoc configuration file path
        /// </summary>
        /// <returns></returns>
        private static string GetShocConfigFilePath()
        {
            // get directory
            var root = ShocRoot.GetOrCreateShocRoot();

            // build the file path
            return Path.Combine(root.FullName, "config.yml");
        }
    }
}