using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Serialization;
using Scriban;
using Shoc.ApiCore.GrpcClient;
using Shoc.Core;
using Shoc.Package.Data;
using Shoc.Package.Model;
using Shoc.Package.Model.BuildTask;
using Shoc.Package.Model.Package;
using Shoc.Package.Templating.Model;
using Shoc.Registry.Grpc.Registries;

namespace Shoc.Package.Services;

/// <summary>
/// The build task service
/// </summary>
public class BuildTaskService
{
    /// <summary>
    /// The name of the default variant
    /// </summary>
    private const string DEFAULT_TEMPLATE_VARIANT = "default";

    /// <summary>
    /// The special generated dockerfile for the build
    /// </summary>
    private const string SHOC_DOCKERFILE_NAME = "Dockerfile.g.shoc";
    
    /// <summary>
    /// The deadline of object creation
    /// </summary>
    private static readonly TimeSpan CREATING_DEADLINE = TimeSpan.FromMinutes(5);
    
    /// <summary>
    /// The build task repository
    /// </summary>
    private readonly IBuildTaskRepository buildTaskRepository;

    /// <summary>
    /// The package repository
    /// </summary>
    private readonly IPackageRepository packageRepository;

    /// <summary>
    /// The template provider
    /// </summary>
    private readonly TemplateProvider templateProvider;

    /// <summary>
    /// The grpc client provider
    /// </summary>
    private readonly IGrpcClientProvider grpcClientProvider;

    /// <summary>
    /// The validation service
    /// </summary>
    private readonly ValidationService validationService;

    /// <summary>
    /// Creates new instance of the service
    /// </summary>
    /// <param name="buildTaskRepository">The build task repository</param>
    /// <param name="packageRepository">The package repository</param>
    /// <param name="templateProvider">The template provider</param>
    /// <param name="grpcClientProvider">The grpc client provider</param>
    /// <param name="validationService">The validation service</param>
    public BuildTaskService(IBuildTaskRepository buildTaskRepository, IPackageRepository packageRepository, TemplateProvider templateProvider, IGrpcClientProvider grpcClientProvider, ValidationService validationService)
    {
        this.buildTaskRepository = buildTaskRepository;
        this.packageRepository = packageRepository;
        this.templateProvider = templateProvider;
        this.grpcClientProvider = grpcClientProvider;
        this.validationService = validationService;
    }
    
    /// <summary>
    /// Gets all the objects
    /// </summary>
    /// <returns></returns>
    public async Task<IEnumerable<BuildTaskModel>> GetAll(string workspaceId)
    {
        // require the parent object
        await this.validationService.RequireWorkspace(workspaceId);

        // get from the storage
        return await this.buildTaskRepository.GetAll(workspaceId);
    }
    
    /// <summary>
    /// Gets the object by id
    /// </summary>
    /// <returns></returns>
    public async Task<BuildTaskModel> GetById(string workspaceId, string id)
    {
        // require the parent object
        await this.validationService.RequireWorkspace(workspaceId);

        // get from the storage
        var result = await this.buildTaskRepository.GetById(workspaceId, id);

        // ensure object exists
        if (result == null)
        {
            throw ErrorDefinition.NotFound().AsException();
        }

        return result;
    }
    
    /// <summary>
    /// Creates the object with given input
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="input">The creation input</param>
    /// <returns></returns>
    public async Task<BuildTaskModel> Create(string workspaceId, BuildTaskCreateModel input)
    {
        // ensure referring to the correct object
        input.WorkspaceId = workspaceId;

        // initial status is creating
        input.Status = BuildTaskStatuses.CREATING;
        
        // initialize deadline
        input.Deadline = DateTime.UtcNow.Add(CREATING_DEADLINE);
        
        // require the parent object
        await this.validationService.RequireWorkspace(workspaceId);

        // require valid user
        await this.validationService.RequireUser(input.UserId);
        
        // validate provider
        ValidateProvider(input.Provider);
        
        // validate target scope
        ValidateScope(input.Scope);
        
        // validate the checksum
        ValidateListingChecksum(input.ListingChecksum);

        // deserialize the manifest
        var manifest = DeserializeManifest(input.Manifest);
        
        // gets the template reference
        var templateReference = GetTemplateReference(manifest.Template);
        
        // assign the template reference
        input.TemplateReference = $"{templateReference.Template}:{templateReference.Variant}";

        // get the build spec schema
        var buildSpecSchema = await this.GetBuildSpecSchema(templateReference.Template, templateReference.Variant);
        
        // validate the spec against the schema
        ValidateBuildSpec(buildSpecSchema, manifest.Spec);

        // get the runtime object
        var runtime = await this.GetRuntime(templateReference.Template, templateReference.Variant);

        // serialize and store the runtime (ensure camel case)
        input.Runtime = JsonConvert.SerializeObject(runtime, new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy
                {
                    ProcessDictionaryKeys = false
                }
            }
        });

        // get the dockerfile template
        var dockerfileTemplate = await this.GetTemplate(templateReference.Template, templateReference.Variant);

        // render dockerfile based on the template and specification
        input.Dockerfile = await RenderDockerfile(dockerfileTemplate, manifest.Spec);

        // gets the registry to store the image
        input.RegistryId = await this.GetDefaultRegistryId(workspaceId);
        
        // create object in the storage
        return await this.buildTaskRepository.Create(workspaceId, input);
    }

    /// <summary>
    /// Updates the bundle by id
    /// </summary>
    /// <param name="userId">The acting user id</param>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="id">The record id</param>
    /// <param name="file">The file</param>
    /// <returns></returns>
    public async Task<BuildTaskModel> UpdateBundleById(string userId, string workspaceId, string id, Stream file)
    {
        // get and require existing object
        var existing = await this.GetById(workspaceId, id);

        // the user performing the operation is different
        if (existing.UserId != userId)
        {
            throw ErrorDefinition.Validation(PackageErrors.USER_MISMATCH).AsException();
        }
        
        // check if status is not in a creating status then error
        if (existing.Status != BuildTaskStatuses.CREATING)
        {
            throw ErrorDefinition.Validation(PackageErrors.UNEXPECTED_OPERATION).AsException();
        }

        // the build has expired in creation status
        if (existing.Deadline < DateTime.UtcNow)
        {
            throw ErrorDefinition.Validation(PackageErrors.EXPIRED_BUILD_TASK).AsException();
        }

        // check the provider
        if (existing.Provider != BuildTaskProviders.REMOTE)
        {
            throw ErrorDefinition.Validation(PackageErrors.INVALID_BUILD_PROVIDER).AsException();
        }

        // empty file for zip
        var zipFile = Path.GetTempFileName();
        
        try
        {
            await using var zipStream = new FileStream(zipFile, FileMode.OpenOrCreate, FileAccess.Write);
            await file.CopyToAsync(zipStream);
        }
        catch (Exception ex)
        {
            // ensure file is deleted
            File.Delete(zipFile);
            
            // rethrow with proper exception
            throw ErrorDefinition.Validation(PackageErrors.UPLOAD_ERROR, ex.Message).AsException();
        }

        // create a root directory for the build
        var rootDir = Directory.CreateTempSubdirectory("build_");

        try
        {
            ZipFile.ExtractToDirectory(zipFile, rootDir.FullName);
        }
        catch (Exception ex)
        {
            // delete directory on error (recursive)
            rootDir.Delete(true);
            
            // rethrow with proper exception
            throw ErrorDefinition.Validation(PackageErrors.UNZIP_ERROR, ex.Message).AsException();
        }
        finally
        {
            // zip file should be deleted anyways
            File.Delete(zipFile);
        }

        // write dockerfile
        await File.WriteAllTextAsync(Path.Combine(rootDir.FullName, SHOC_DOCKERFILE_NAME), existing.Dockerfile);

        // update the status
        return await this.buildTaskRepository.UpdateById(workspaceId, id, new BuildTaskUpdateModel
        {
            WorkspaceId = workspaceId,
            Id = id,
            Status = BuildTaskStatuses.BUILDING,
            Deadline = DateTime.UtcNow.AddHours(10),
            ErrorCode = null,
            PackageId = null
        });
    }

    /// <summary>
    /// Gets the registry to store the package
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <returns></returns>
    protected async Task<string> GetDefaultRegistryId(string workspaceId)
    {
        // try getting object
        try {
            var result = await this.grpcClientProvider
                .Get<WorkspaceDefaultRegistryServiceGrpc.WorkspaceDefaultRegistryServiceGrpcClient>()
                .DoAuthorized(async (client, metadata) => await client.GetByWorkspaceIdAsync(new GetWorkspaceDefaultRegistryRequest{WorkspaceId = workspaceId}, metadata));

            return result.Registry.Id;
        }
        catch(Exception)
        {
            throw ErrorDefinition.Validation(PackageErrors.INVALID_REGISTRY).AsException();
        }
    }

    /// <summary>
    /// Renders the given template with the given spec
    /// </summary>
    /// <param name="dockerfileTemplate">The template content</param>
    /// <param name="spec">The specification to render</param>
    /// <returns></returns>
    private static async Task<string> RenderDockerfile(string dockerfileTemplate, Dictionary<string, object> spec)
    {
        try
        {
            // parse the template file
            var parsed = Template.Parse(dockerfileTemplate);

            // render with spec
            return await parsed.RenderAsync(spec);
        }
        catch (Exception e)
        {
            throw ErrorDefinition.Validation(PackageErrors.RENDER_FAILURE, e.Message).AsException();
        }
    }

    /// <summary>
    /// Gets the valid runtime for the template
    /// </summary>
    /// <param name="name">The template name</param>
    /// <param name="variant">The template variant</param>
    /// <returns></returns>
    protected async Task<TemplateRuntimeModel> GetRuntime(string name, string variant)
    {
        try
        {
            return await this.templateProvider.GetRuntimeByName(name, variant);
        }
        catch (Exception e)
        {
            throw ErrorDefinition.Validation(PackageErrors.INVALID_RUNTIME, e.Message).AsException();
        }
    }
    
    /// <summary>
    /// Gets the valid runtime for the template
    /// </summary>
    /// <param name="name">The template name</param>
    /// <param name="variant">The template variant</param>
    /// <returns></returns>
    protected async Task<string> GetTemplate(string name, string variant)
    {
        try
        {
            return await this.templateProvider.GetTemplateByName(name, variant);
        }
        catch (Exception e)
        {
            throw ErrorDefinition.Validation(PackageErrors.INVALID_DOCKERFILE_TEMPLATE, e.Message).AsException();
        }
    }

    /// <summary>
    /// Gets the build spec schema
    /// </summary>
    /// <param name="name">The name of the template</param>
    /// <param name="variant">The template variant</param>
    /// <returns></returns>
    protected async Task<JSchema> GetBuildSpecSchema(string name, string variant)
    {
        try
        {
            return await this.templateProvider.GetBuildSpecSchemaByName(name, variant);
        }
        catch (Exception)
        {
            throw ErrorDefinition.Validation(PackageErrors.INVALID_BUILD_SPEC).AsException();
        }
    }

    /// <summary>
    /// Validates the build spec object against the schema
    /// </summary>
    /// <param name="schema">The schema</param>
    /// <param name="spec">The spec</param>
    protected void ValidateBuildSpec(JSchema schema, Dictionary<string, object> spec)
    {
        // no spec was provided
        if (spec == null)
        {
            throw ErrorDefinition.Validation(PackageErrors.INVALID_BUILD_SPEC).AsException();
        }
        
        // parse the object
        var specObject = JObject.FromObject(spec);
        
        // check if not valid
        if (!specObject.IsValid(schema, out IList<ValidationError> errors))
        {
            throw ErrorDefinition.Validation(PackageErrors.INVALID_BUILD_SPEC, errors.FirstOrDefault()?.Message).AsException();
        }
    }
    
    /// <summary>
    /// Validate object provider
    /// </summary>
    /// <param name="provider">The provider to validate</param>
    protected static void ValidateProvider(string provider)
    {
        // make sure valid status
        if (BuildTaskProviders.ALL.Contains(provider))
        {
            return;
        }

        throw ErrorDefinition.Validation(PackageErrors.INVALID_BUILD_PROVIDER).AsException();
    }
    
    /// <summary>
    /// Validate object scope
    /// </summary>
    /// <param name="scope">The scope to validate</param>
    protected static void ValidateScope(string scope)
    {
        // make sure valid status
        if (PackageScopes.ALL.Contains(scope))
        {
            return;
        }

        throw ErrorDefinition.Validation(PackageErrors.INVALID_PACKAGE_SCOPE).AsException();
    }

    /// <summary>
    /// Validates the listing checksum
    /// </summary>
    /// <param name="checksum">The checksum</param>
    protected static void ValidateListingChecksum(string checksum)
    {
        if (string.IsNullOrWhiteSpace(checksum))
        {
            throw ErrorDefinition.Validation(PackageErrors.INVALID_LISTING_CHECKSUM).AsException();
        }
    }

    /// <summary>
    /// Deserialize the manifest json to a proper format
    /// </summary>
    /// <param name="manifest">The manifest string</param>
    /// <returns></returns>
    protected static BuildManifestModel DeserializeManifest(string manifest)
    {
        // do not allow empty manifest
        if (string.IsNullOrWhiteSpace(manifest))
        {
            throw ErrorDefinition.Validation(PackageErrors.INVALID_MANIFEST).AsException();
        }

        try
        {
            // using Newtonsoft to make sure Dictionary<string, object> has simple values not Json Elements
            return JsonConvert.DeserializeObject<BuildManifestModel>(manifest);
        }
        catch (Exception e)
        {
            throw ErrorDefinition.Validation(PackageErrors.INVALID_MANIFEST, e.Message).AsException();
        }
    }

    /// <summary>
    /// Gets the template reference
    /// </summary>
    /// <param name="template">The template string</param>
    /// <returns></returns>
    protected static TemplateReference GetTemplateReference(string template)
    {
        // ensure no empty template
        if (string.IsNullOrWhiteSpace(template))
        {
            throw ErrorDefinition.Validation(PackageErrors.INVALID_TEMPLATE).AsException();
        }

        // split template by parts
        var parts = template.Split(':', 2, StringSplitOptions.RemoveEmptyEntries);

        // the name of the template
        var name = parts[0];

        // the variant
        var variant = DEFAULT_TEMPLATE_VARIANT;

        // check if there is a second part and it's not empty
        if (parts.Length > 1 && !string.IsNullOrWhiteSpace(parts[1]))
        {
            variant = parts[1];
        }

        return new TemplateReference
        {
            Template = name,
            Variant = variant
        };
    }

}