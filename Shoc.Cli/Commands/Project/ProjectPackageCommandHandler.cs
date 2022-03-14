using System;
using System.Collections.Generic;
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
            // get the manifest
            var manifest = await this.RequireManifest();

            // make sure input is valid
            if (manifest.Build?.Input == null)
            {
                throw ErrorDefinition.Validation(CliErrors.MISSING_MANIFEST_BUILD_INPUT).AsException();
            }

            // create serializer
            var serializer = new SerializerBuilder()
                .WithNamingConvention(HyphenatedNamingConvention.Instance)
                .Build();


            // get all required files if everything is OK
            var files = this.GetRequiredFiles(manifest.Build.Input);

            // get checksum
            var checksum = GetChecksum(files);

            // get the project instance
            var project = await this.WithProject((_, p) => Task.FromResult(p));

            // create package in authorized context
            var package = await this.authService.DoAuthorized(this.Profile, (profile, auth) =>
            {
                // get client
                var client = this.clientService.Builder(profile);

                // create package with the client
                return client.CreatePackage(auth.AccessToken, project.Id, new CreatePackageInput
                {
                    ProjectId = project.Id,
                    BuildSpec = serializer.Serialize(manifest.Build),
                    Status = PackageStatuses.INIT,
                    ListingChecksum = checksum
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
                    zip.CreateEntryFromFile(file.Path, Path.GetRelativePath(this.Directory.FullName, file.Path), CompressionLevel.Optimal);
                }
            }

            

            Console.WriteLine($"Computing files to package. {files.Count} files were identified. Archived to {zipFile} and checksum is {checksum}");
            return 0;
        }

        /// <summary>
        /// Gets all the required files
        /// </summary>
        /// <param name="input">The input specification</param>
        /// <returns></returns>
        private IList<FileEntry> GetRequiredFiles(BuildInputSpec input)
        {
            // get all the paths in the copy section
            var files = input?.Copy?.Select(cp => cp.From) ?? Enumerable.Empty<string>();

            // create new matcher
            var matcher = new Matcher();

            // add all patterns for matching
            matcher.AddIncludePatterns(files);

            // get all the files
            var all = matcher.GetResultsInFullPath(this.Directory.FullName).Select(path => new FileInfo(path)).ToList();

            // if there is any file that does not exist
            if (all.Any(file => !file.Exists))
            {
                ErrorDefinition.Validation(CliErrors.MISSING_REQUIRED_FILES).AsException();
            }

            // check if there is a file that is outside of the context directory
            if (all.Any(file => !file.FullName.StartsWith(this.Directory.FullName)))
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
    }
}