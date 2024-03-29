﻿namespace Shoc.Builder.Model
{
    /// <summary>
    /// The builder error definitions
    /// </summary>
    public class BuilderErrors
    {
        /// <summary>
        /// The invalid name
        /// </summary>
        public const string INVALID_NAME = "BUILDER_INVALID_NAME";

        /// <summary>
        /// The invalid type
        /// </summary>
        public const string INVALID_TYPE = "BUILDER_INVALID_TYPE";

        /// <summary>
        /// The existing name
        /// </summary>
        public const string EXISTING_NAME = "BUILDER_EXISTING_NAME";

        /// <summary>
        /// The invalid project error
        /// </summary>
        public const string INVALID_PROJECT = "BUILDER_INVALID_PROJECT";

        /// <summary>
        /// The invalid owner
        /// </summary>
        public const string INVALID_OWNER = "BUILDER_INVALID_OWNER";

        /// <summary>
        /// No registry is available
        /// </summary>
        public const string NO_REGISTRY = "BUILDER_NO_REGISTRY";

        /// <summary>
        /// The invalid registry name is given
        /// </summary>
        public const string INVALID_REGISTRY_NAME = "BUILDER_INVALID_REGISTRY_NAME";

        /// <summary>
        /// The invalid registry uri
        /// </summary>
        public const string INVALID_REGISTRY_URI = "BUILDER_INVALID_REGISTRY_URI";

        /// <summary>
        /// The invalid repository uri
        /// </summary>
        public const string INVALID_REPOSITORY_URI = "BUILDER_INVALID_REPOSITORY_URI";

        /// <summary>
        /// The invalid registry credentials
        /// </summary>
        public const string INVALID_REGISTRY_CREDENTIALS = "BUILDER_INVALID_REGISTRY_CREDENTIALS";

        /// <summary>
        /// The invalid kubeconfig
        /// </summary>
        public const string INVALID_KUBE_CONFIG = "BUILDER_INVALID_KUBE_CONFIG";

        /// <summary>
        /// The invalid access to the object
        /// </summary>
        public const string ACCESS_DENIED = "BUILDER_ACCESS_DENIED";

        /// <summary>
        /// The unknown error
        /// </summary>
        public const string UNKNOWN_ERROR = "BUILDER_UNKNOWN_ERROR";

        /// <summary>
        /// The dockerfile error
        /// </summary>
        public const string DOCKERFILE_ERROR = "BUILDER_DOCKERFILE_FAILED_ERROR";
    }
}