using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.DataProtection;
using Shoc.Cli.Utility;

namespace Shoc.Cli.Services
{
    /// <summary>
    /// The encrypted storage service
    /// </summary>
    public class EncryptedStorage
    {
        /// <summary>
        /// The storage protector purpose
        /// </summary>
        private const string PROTECTOR_PURPOSE = "storage";

        /// <summary>
        /// The data protection provider instance
        /// </summary>
        private readonly IDataProtectionProvider dataProtectionProvider;

        /// <summary>
        /// The root folder for storing values
        /// </summary>
        private readonly string root;

        /// <summary>
        /// Creates new instance of encrypted storage
        /// </summary>
        /// <param name="dataProtectionProvider">The data protection provider</param>
        public EncryptedStorage(IDataProtectionProvider dataProtectionProvider)
        {
            this.dataProtectionProvider = dataProtectionProvider;
            this.root = Path.Combine(ShocRoot.GetOrCreateShocRoot().FullName, "e-storage");
        }

        /// <summary>
        /// Gets the value by group and key
        /// </summary>
        /// <param name="group">The group of keys</param>
        /// <param name="key">The key for lookup</param>
        /// <returns></returns>
        public async Task<string> Get(string group, string key)
        {
            // the group root
            var groupRoot = Path.Combine(this.root, group);

            // make sure folder exists
            Directory.CreateDirectory(groupRoot);

            // value file
            var valueFile = Path.Combine(groupRoot, $"{key}.value");

            // does not exist
            if (!File.Exists(valueFile))
            {
                return null;
            }

            // create protector
            var protector = this.dataProtectionProvider.CreateProtector(PROTECTOR_PURPOSE);

            // read the value
            var value = protector.Unprotect(await File.ReadAllBytesAsync(valueFile));

            // stringify the value
            return Encoding.UTF8.GetString(value);
        }

        /// <summary>
        /// Stores the encrypted value by key and group
        /// </summary>
        /// <param name="group">The key group</param>
        /// <param name="key">The kye</param>
        /// <param name="value">The value</param>
        /// <returns></returns>
        public async Task Set(string group, string key, string value)
        {
            // the group root
            var groupRoot = Path.Combine(this.root, group);

            // make sure folder exists
            Directory.CreateDirectory(groupRoot);

            // value file
            var valueFile = Path.Combine(groupRoot, $"{key}.value");

            // create protector
            var protector = this.dataProtectionProvider.CreateProtector(PROTECTOR_PURPOSE);

            await File.WriteAllBytesAsync(valueFile, protector.Protect(Encoding.UTF8.GetBytes(value)));
        }

        /// <summary>
        /// Removes the encrypted value by key and group
        /// </summary>
        /// <param name="group">The key group</param>
        /// <param name="key">The kye</param>
        /// <returns></returns>
        public Task Remove(string group, string key)
        {
            // the group root
            var groupRoot = Path.Combine(this.root, group);

            // make sure folder exists
            Directory.CreateDirectory(groupRoot);

            // value file
            var valueFile = Path.Combine(groupRoot, $"{key}.value");
            
            // delete the file
            File.Delete(valueFile);

            // completed task
            return Task.CompletedTask;
        }
    }
}