using System;
using Microsoft.AspNetCore.Authorization;

namespace Shoc.ApiCore.Protection
{
    /// <summary>
    /// The filtering attribute makes sure the bearer authentication 
    /// </summary>
    public class BearerOnlyAttribute : Attribute, IAuthorizeData
    {
        /// <summary>
        /// The policy required to match
        /// </summary>
        string IAuthorizeData.Policy
        {
            get => null;
            set { }
        }

        /// <summary>
        /// The role requirement from authorization context
        /// </summary>
        string IAuthorizeData.Roles
        {
            get => null;
            set { }
        }

        /// <summary>
        /// The authentication schemes
        /// </summary>
        public string AuthenticationSchemes { get; set; }
        
        /// <summary>
        /// Creates new bearer guard
        /// </summary>
        /// <param name="schemes">The bearer scheme names</param>
        public BearerOnlyAttribute(params string[] schemes)
        {
            this.AuthenticationSchemes = schemes.Length == 0 ? "Bearer" : string.Join(",", schemes);
        }

        /// <summary>
        /// Creates new bearer guard
        /// </summary>
        /// <param name="scheme">The bearer scheme name</param>
        public BearerOnlyAttribute(string scheme = null)
        {
            this.AuthenticationSchemes = string.IsNullOrWhiteSpace(scheme) ? "Bearer" : scheme;
        }
    }
}