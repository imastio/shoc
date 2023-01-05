using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Imast.Ext.Core;
using Microsoft.AspNetCore.DataProtection;
using Shoc.ApiCore;
using Shoc.Builder.Data;
using Shoc.Builder.Model;
using Shoc.Builder.Model.Registry;
using Shoc.Core;
using Shoc.Identity.Model;
using Shoc.ModelCore;

namespace Shoc.Builder.Services
{
    /// <summary>
    /// The docker registry service implementation
    /// </summary>
    public class DockerRegistryService
    {
        /// <summary>
        /// The name validation regex
        /// </summary>
        private static readonly Regex NAME_REGEX = new("^[a-zA-Z0-9_]+$");

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
        /// <param name="principal">The requesting principal</param>
        /// <param name="query">The query to lookup</param>
        /// <returns></returns>
        public Task<IEnumerable<DockerRegistry>> GetBy(ShocPrincipal principal, DockerRegistryQuery query)
        {
            // gets all the entries by owner
            return this.dockerRegistryRepository.GetBy(query);
        }

        /// <summary>
        /// Gets the docker registry by id
        /// </summary>
        /// <param name="principal">The current principal</param>
        /// <param name="id">The id of registry</param>
        /// <returns></returns>
        public async Task<DockerRegistry> GetById(ShocPrincipal principal, string id)
        {
            // try load the result
            var result = await this.dockerRegistryRepository.GetById(id);

            // not found
            if (result == null)
            {
                throw ErrorDefinition.NotFound().AsException();
            }
            
            // require to be either administrator or owner
            AccessGuard.Require(() => UserTypes.ESCALATED.Contains(principal.Type));

            // the result
            return result;
        }

        /// <summary>
        /// Creates a docker registry with given input
        /// </summary>
        /// <param name="principal">The authenticated principal</param>
        /// <param name="input">The registry creation input</param>
        /// <returns></returns>
        public async Task<DockerRegistry> Create(ShocPrincipal principal, CreateDockerRegistry input)
        {
            // name should be given
            if (string.IsNullOrEmpty(input.Name))
            {
                throw ErrorDefinition.Validation(BuilderErrors.INVALID_NAME).AsException();
            }

            // check if name does not pass the pattern
            if (!NAME_REGEX.IsMatch(input.Name))
            {
                throw ErrorDefinition.Validation(BuilderErrors.INVALID_NAME).AsException();
            }

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

            // check if repository is missing
            if (string.IsNullOrWhiteSpace(input.Repository))
            {
                throw ErrorDefinition.Validation(BuilderErrors.INVALID_REPOSITORY_URI).AsException();
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
                input.EncryptedPassword = protector.Protect(input.PasswordPlaintext);
            }

            // try load an existing item with the name
            var existing = await this.dockerRegistryRepository.GetBy(new DockerRegistryQuery
            {
                Name = input.Name
            });

            // there is an object with the name
            if (existing.Any())
            {
                throw ErrorDefinition.Validation(BuilderErrors.EXISTING_NAME).AsException();
            }

            // create the registry
            return await this.dockerRegistryRepository.Create(input);
        }

        /// <summary>
        /// Deletes the registry by given id
        /// </summary>
        /// <param name="principal">The principal</param>
        /// <param name="id">The id of registry</param>
        /// <returns></returns>
        public async Task<DockerRegistry> DeleteById(ShocPrincipal principal, string id)
        {
            // get the object by id
            var result = await this.GetById(principal, id);

            // if object is available delete from repository
            return await this.dockerRegistryRepository.DeleteById(result.Id);
        }
    }
}