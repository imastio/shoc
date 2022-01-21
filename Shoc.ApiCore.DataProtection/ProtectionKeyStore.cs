using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Microsoft.AspNetCore.DataProtection.Repositories;
using Shoc.DataProtection;

namespace Shoc.ApiCore.DataProtection
{
    /// <summary>
    /// The protection key store
    /// </summary>
    public class ProtectionKeyStore : IXmlRepository
    {
        /// <summary>
        /// The protection key repository
        /// </summary>
        private readonly IProtectionKeyRepository keyRepository;

        /// <summary>
        /// The protection key store
        /// </summary>
        /// <param name="keyRepository">The key repository</param>
        public ProtectionKeyStore(IProtectionKeyRepository keyRepository)
        {
            this.keyRepository = keyRepository;
        }

        /// <summary>
        /// Gets all key elements
        /// </summary>
        /// <returns></returns>
        public IReadOnlyCollection<XElement> GetAllElements()
        {
            // get all keys
            return this.keyRepository.GetAll().Result
                .Where(key => !string.IsNullOrWhiteSpace(key.Xml))
                .Select(key => XElement.Parse(key.Xml))
                .ToList()
                .AsReadOnly();
        }

        /// <summary>
        /// Stores the given element
        /// </summary>
        /// <param name="element">The XML element</param>
        /// <param name="friendlyName">The friendly name</param>
        public void StoreElement(XElement element, string friendlyName)
        {
            var _ = this.keyRepository.Create(new ProtectionKeyModel
            {
                FriendlyName = friendlyName,
                Xml = element.ToString(SaveOptions.DisableFormatting)
            });
        }
    }
}