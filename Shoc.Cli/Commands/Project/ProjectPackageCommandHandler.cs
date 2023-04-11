using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.FileSystemGlobbing;
using Shoc.Builder.Model.Package;
using Shoc.Cli.Model;
using Shoc.Cli.Services;
using Shoc.Cli.Utility;
using Shoc.Core;
using Shoc.ModelCore;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Shoc.Cli.Commands.Project
{
    /// <summary>
    /// The package project command handler
    /// </summary>
    public class ProjectPackageCommandHandler : ProjectCommandHandlerBase
    {
        /// <summary>
        /// The shoc build spec manifest file name
        /// </summary>
        private const string BUILDSPEC_FILE = "shoc-build.yml";

        /// <summary>
        /// The shoc ignore file for files
        /// </summary>
        private const string SHOCIGNORE_FILE = ".shocignore";

        /// <summary>
        /// The project version (package)
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// The project name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The directory path
        /// </summary>
        public DirectoryInfo TargetDirectory { get; set; }

        /// <summary>
        /// Creates new instance of command handler
        /// </summary>
        /// <param name="clientService">The client service</param>
        /// <param name="authService">The auth service</param>
        public ProjectPackageCommandHandler(ClientService clientService, AuthService authService) : base(clientService, authService)
        {
        }

        /// <summary>
        /// Implementation of project command invocation
        /// </summary>
        /// <param name="context">The invocation context</param>
        /// <returns></returns>
        public override async Task<int> InvokeAsync(InvocationContext context)
        {
            if (string.IsNullOrEmpty(this.Name))
            {
                ErrorDefinition.Validation(CliErrors.MISSING_PROJECT_NAME);
            }

            this.TargetDirectory ??= new DirectoryInfo("./");

            if (string.IsNullOrEmpty(this.TargetDirectory.FullName))
            {
                ErrorDefinition.Validation(CliErrors.MISSING_PROJECT_DIRECTORY);
            }

            // get the project
            var project = await this.RequireProject(this.Name);

            // get the build manifest
            var manifest = await this.GetBuildManifest();

            // create serializer
            var serializer = new SerializerBuilder()
                .WithNamingConvention(HyphenatedNamingConvention.Instance)
                .Build();

            // get all required files if everything is OK
            var files = await this.GetRequiredFiles();

            // get checksum
            var checksum = GetChecksum(files);

            // create package in authorized context
            var package = await this.authService.DoAuthorized(this.Profile, (profile, auth) =>
            {
                // get client
                var client = this.clientService.Builder(profile);

                // create package with the client
                return client.CreatePackage(auth.AccessToken, project.Id, new CreatePackageInput
                {
                    ProjectId = project.Id,
                    BuildSpec = manifest == null ? string.Empty : serializer.Serialize(manifest),
                    Status = PackageStatuses.INIT,
                    ListingChecksum = checksum,
                });
            });

            Console.WriteLine($"Create package ({package.Id}) for the current project");

            // new temporary file
            var zipFile = Path.GetTempFileName();

            // create new zip archive
            using (var zip = ZipFile.Open(zipFile, ZipArchiveMode.Update))
            {
                // adding files to archive
                foreach (var file in files)
                {
                    zip.CreateEntryFromFile(file.Path, Path.GetRelativePath(this.TargetDirectory.FullName, file.Path), CompressionLevel.Optimal);
                }
            }

            // create bundle authorized
            _ = await this.authService.DoAuthorized(this.Profile, async (profile, auth) =>
            {
                // get the client
                var client = this.clientService.Builder(profile);

                // send the file and get the reference back
                return await client.UploadBundle(auth.AccessToken, project.Id, package.Id, zipFile);
            });

            // delete temporary zip file
            File.Delete(zipFile);

            var version = this.Version ?? "latest";

            // create bundle authorized
            _ = await this.authService.DoAuthorized(this.Profile, (profile, auth) =>
            {
                // get the client
                var client = this.clientService.Builder(profile);

                // build the package
                return client.BuildPackage(auth.AccessToken, project.Id, package.Id, version);
            });

            context.Console.WriteLine($"Computing files to package. {files.Count} files were identified. Archived to {zipFile} and checksum is {checksum}");
            context.Console.WriteLine("");
            context.Console.WriteLine("To run the project, use the following command:");
            context.Console.WriteLine($"\tshocctl project run -n {project.Name} -v {version}");
            context.Console.WriteLine("");
            context.Console.WriteLine("To pass additional command-line arguments specify -a parameter as follows:");
            context.Console.WriteLine($"\tshocctl project run -n {project.Name} -v {version} -a <ARGS>");
            context.Console.WriteLine("");

            return 0;
        }

        /// <summary>
        /// Gets all the required files
        /// </summary>
        /// <returns></returns>
        private async Task<IList<FileEntry>> GetRequiredFiles()
        {
            // get all the paths in the copy section
            //var files = input?.(cp => cp.From) ?? Enumerable.Empty<string>();
            var files = new List<string>() { "**" };
            
            // get shoc ignore 
            var shocIgnore = await this.GetShocIgnore();

            // create new matcher
            var matcher = new Matcher();

            // add all patterns for matching
            matcher.AddIncludePatterns(files);

            // if shoc ignore exists then add exclude patterns
            if (shocIgnore != null)
            {
                matcher.AddExcludePatterns(shocIgnore);
            }

            // get all the files
            var all = matcher.GetResultsInFullPath(this.TargetDirectory.FullName).Select(path => new FileInfo(path)).ToList();

            // if there is any file that does not exist
            if (all.Any(file => !file.Exists))
            {
                ErrorDefinition.Validation(CliErrors.MISSING_REQUIRED_FILES).AsException();
            }

            // check if there is a file that is outside of the context directory
            if (all.Any(file => !file.FullName.StartsWith(this.TargetDirectory.FullName)))
            {
                ErrorDefinition.Validation(CliErrors.FILE_OUTSIDE_CONTEXT).AsException();
            }

            // create set of file entries
            return all.Select(file => new FileEntry
            {
                Path = file.FullName,
                Size = file.Length,
                LastModified = file.LastWriteTimeUtc.ToBinary()
            }).ToList();
        }

        /// <summary>
        /// Gets the checksum of the set of files
        /// </summary>
        /// <param name="entries">The entries to consider</param>
        /// <returns></returns>
        private static string GetChecksum(IEnumerable<FileEntry> entries)
        {
            // collect all the entries as strings
            var builder = new StringBuilder();

            // append all the file identities (size, modified time, path)
            foreach (var entry in entries)
            {
                builder.AppendLine($"Sz: {entry.Size}, Mt: {entry.LastModified}, Pt: {entry.Path.Replace("\\", "/")}");
            }

            // as checksum string
            return $"CA1-{builder.ToString().ToSafeSha256()}";
        }

        /// <summary>
        /// Gets the manifest if exists
        /// </summary>
        /// <returns></returns>
        protected async Task<ShocManifest> GetBuildManifest()
        {
            // the path to manifest file
            var path = Path.Combine(this.TargetDirectory.FullName, BUILDSPEC_FILE);

            // check if manifest file exists
            return File.Exists(path) ? Yml.Deserialize<ShocManifest>(await File.ReadAllTextAsync(path)) : null;
        }

        /// <summary>
        /// Gets the manifest if exists
        /// </summary>
        /// <returns></returns>
        protected async Task<IEnumerable<string>> GetShocIgnore()
        {
            // the path to manifest file
            var path = Path.Combine(this.TargetDirectory.FullName, SHOCIGNORE_FILE);

            // get file data
            var fileData = File.Exists(path) ? (await File.ReadAllLinesAsync(path)).ToList() : new List<string>();

            // add known files
            fileData.AddRange(this.GetKnownIgnoreFiles());

            // check if manifest file exists
            return fileData;
        }

        /// <summary>
        /// Gets the manifest if exists
        /// </summary>
        /// <returns></returns>
        protected async Task<ShocManifest> RequireBuildManifest()
        {
            // try get manifest
            var manifest = await this.GetBuildManifest();

            // make sure manifest exists
            if (manifest == null)
            {
                throw ErrorDefinition.Validation(CliErrors.MISSING_MANIFEST, "The build manifest is missing.").AsException();
            }

            return manifest;
        }

        /// <summary>
        /// Saves the given manifest to the project file
        /// </summary>
        /// <param name="manifest">The manifest to save</param>
        /// <returns></returns>
        protected async Task<ShocManifest> SaveManifest(ShocManifest manifest)
        {
            // the path to manifest file
            var path = Path.Combine(this.TargetDirectory.FullName, BUILDSPEC_FILE);

            // write the manifest to the directory
            await File.WriteAllTextAsync(path, Yml.Serialize(manifest));

            // get saved object
            return await this.GetBuildManifest();
        }

        /// <summary>
        /// Gets the manifest if exists
        /// </summary>
        /// <returns></returns>
        protected IEnumerable<string> GetKnownIgnoreFiles()
        {
            return new List<string>
            {
                ".git"
            };
        }
    }
}