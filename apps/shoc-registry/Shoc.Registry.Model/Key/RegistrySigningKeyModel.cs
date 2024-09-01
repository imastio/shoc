using System;

namespace Shoc.Registry.Model.Key;

/// <summary>
/// The registry signin key model
/// </summary>
public class RegistrySigningKeyModel
{
    /// <summary>
    /// The id of the key
    /// </summary>
    public string Id { get; set; }
    
    /// <summary>
    /// The registry id
    /// </summary>
    public string RegistryId { get; set; }
    
    /// <summary>
    /// The key id (kid)
    /// </summary>
    public string KeyId { get; set; }
    
    /// <summary>
    /// The cryptographical algorithm
    /// </summary>
    public string Algorithm { get; set; }
    
    /// <summary>
    /// The encrypted payload of the key material
    /// </summary>
    public string PayloadEncrypted { get; set; }
    
    /// <summary>
    /// Indicates if the key is X509 certificate
    /// </summary>
    public bool IsX509Certificate { get; set; }
    
    /// <summary>
    /// The usage scope of the key
    /// </summary>
    public string Usage { get; set; }
    
    /// <summary>
    /// The creation time
    /// </summary>
    public DateTime Created { get; set; }
    
    /// <summary>
    /// The update time
    /// </summary>
    public DateTime Updated { get; set; }
}