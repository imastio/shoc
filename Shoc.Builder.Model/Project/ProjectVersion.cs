﻿namespace Shoc.Builder.Model.Project
{
    /// <summary>
    /// The package version definition
    /// </summary>
    public class ProjectVersion
    {
        /// <summary>
        /// The project id
        /// </summary>
        public string ProjectId { get; set; }

        /// <summary>
        /// The version string
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// The package id
        /// </summary>
        public string PackageId { get; set; }
    }
}