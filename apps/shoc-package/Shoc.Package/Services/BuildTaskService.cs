using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using Shoc.ApiCore.GrpcClient;
using Shoc.Core;
using Shoc.Package.Data;
using Shoc.Package.Model;
using Shoc.Package.Model.BuildTask;
using Shoc.Package.Model.Package;
using Shoc.Package.Templating.Model;

namespace Shoc.Package.Services;

/// <summary>
/// The build task service
/// </summary>
public class BuildTaskService
{
    /// <summary>
    /// The name of the default variant
    /// </summary>
    public const string DEFAULT_TEMPLATE_VARIANT = "default";
    
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
    /// The schema provider
    /// </summary>
    private readonly SchemaProvider schemaProvider;

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
    /// <param name="schemaProvider">The schema provider</param>
    /// <param name="templateProvider">The template provider</param>
    /// <param name="grpcClientProvider">The grpc client provider</param>
    /// <param name="validationService">The validation service</param>
    public BuildTaskService(IBuildTaskRepository buildTaskRepository, IPackageRepository packageRepository, SchemaProvider schemaProvider, TemplateProvider templateProvider, IGrpcClientProvider grpcClientProvider, ValidationService validationService)
    {
        this.buildTaskRepository = buildTaskRepository;
        this.packageRepository = packageRepository;
        this.schemaProvider = schemaProvider;
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
        return await this.buildTaskRepository.GetById(workspaceId, id);
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
        ValidateScope(input.TargetScope);
        
        // validate the checksum
        ValidateListingChecksum(input.ListingChecksum);

        // deserialize the manifest
        var manifest = DeserializeManifest(input.Manifest);

        // gets the template reference
        var templateReference = GetTemplateReference(manifest.Template);

        // get the build spec schema
        var buildSpecSchema = await this.GetBuildSpecSchema(templateReference.Template, templateReference.Variant);
        
        // validate the spec against the schema
        ValidateBuildSpec(buildSpecSchema, manifest.Spec);
        
        input.
        
        // try getting object by name
        var existing = await this.secretRepository.GetByName(workspaceId, input.Name);

        // if object by name exists
        if (existing != null)
        {
            throw ErrorDefinition.Validation(SecretErrors.EXISTING_NAME).AsException();
        }
        
        // create a protector
        var protector = this.protectionProvider.Create();
        
        // encrypt if needed
        input.Value = input.Encrypted ? protector.Protect(input.Value) : input.Value;
        
        // create object in the storage
        return await this.secretRepository.Create(workspaceId, input);
    }
    
    /// <summary>
    /// Gets the build spec schema
    /// </summary>
    /// <param name="name">The name of the template</param>
    /// <param name="variant">The template variant</param>
    /// <returns></returns>
    protected Task<JSchema> GetBuildSpecSchema(string name, string variant)
    {
        try
        {
            return this.templateProvider.GetBuildSpecSchemaByName(name, variant);
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