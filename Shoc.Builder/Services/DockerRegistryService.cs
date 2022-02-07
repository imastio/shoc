using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Imast.Ext.Core;
using Microsoft.AspNetCore.DataProtection;
using Shoc.Builder.Data;
using Shoc.Builder.Model;
using Shoc.Core;

namespace Shoc.Builder.Services
{
    /// <summary>
    /// The docker registry service implementation
    /// </summary>
    public class DockerRegistryService
    {
        /// <summary>
        /// The docker registry repository
        /// </summary>
        private readonly IDockerRegistryRepository dockerRegistryRepository;

        /// <summary>
        /// The data protection provider 
        /// </summary>
        private readonly IDataProtectionProvider dataProtectionProvider;

        /// <summary>
        /// Creates new instance of docker registry service
        /// </summary>
        /// <param name="dockerRegistryRepository">The docker registry repository</param>
        /// <param name="dataProtectionProvider">The data protection provider</param>
        public DockerRegistryService(IDockerRegistryRepository dockerRegistryRepository, IDataProtectionProvider dataProtectionProvider)
        {
            this.dockerRegistryRepository = dockerRegistryRepository;
            this.dataProtectionProvider = dataProtectionProvider;
        }

        /// <summary>
        /// Gets all the docker registry instances
        /// </summary>
        /// <returns></returns>
        public Task<IEnumerable<DockerRegistry>> GetAll()
        {
            return this.dockerRegistryRepository.GetAll();
        }

        /// <summary>
        /// Gets the docker registry by id
        /// </summary>
        /// <param name="id">The id of registry</param>
        /// <returns></returns>
        public Task<DockerRegistry> GetById(string id)
        {
            return this.dockerRegistryRepository.GetById(id);
        }

        /// <summary>
        /// Creates a docker registry with given input
        /// </summary>
        /// <param name="input">The registry creation input</param>
        /// <returns></returns>
        public Task<DockerRegistry> Create(CreateDockerRegistry input)
        {
            // check if registry is missing
            if (string.IsNullOrWhiteSpace(input.RegistryUri))
            {
                throw ErrorDefinition.Validation(BuilderErrors.INVALID_REGISTRY_URI).AsException();
            }

            // ensure trailing slash 
            if (!input.RegistryUri.EndsWith("/"))
            {
                input.RegistryUri = $"{input.RegistryUri}/";
            }

            // make sure to have valid uri
            if (!Uri.TryCreate(input.RegistryUri, UriKind.RelativeOrAbsolute, out _))
            {
                throw ErrorDefinition.Validation(BuilderErrors.INVALID_REGISTRY_URI).AsException();
            }

            // if username is given but password is missing raise an error
            if (input.Username.IsNotBlank() && input.PasswordPlaintext.IsBlank())
            {
                throw ErrorDefinition.Validation(BuilderErrors.INVALID_REGISTRY_CREDENTIALS).AsException();
            }

            // if username is not given but password is given raise an error
            if (input.Username.IsBlank() && input.PasswordPlaintext.IsNotBlank())
            {
                throw ErrorDefinition.Validation(BuilderErrors.INVALID_REGISTRY_CREDENTIALS).AsException();
            }

            // create a protector
            var protector = this.dataProtectionProvider.CreateProtector(BuilderProtection.REGISTRY_CREDENTIALS);

            // in case if password is given encrypt it
            if (input.PasswordPlaintext.IsNotBlank())
            {
                input.EncryptedPassword = protector.Protect(Encoding.UTF8.GetBytes(input.PasswordPlaintext));
            }

            // create the registry
            return this.dockerRegistryRepository.Create(input);
        }

        /// <summary>
        /// Deletes the registry by given id
        /// </summary>
        /// <param name="id">The id of registry</param>
        /// <returns></returns>
        public Task<DockerRegistry> DeleteById(string id)
        {
            return this.dockerRegistryRepository.DeleteById(id);
        }
    }
}