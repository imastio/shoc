namespace Shoc.Registry.Model.TokenSpec;

/// <summary>
/// The basic credentials
/// </summary>
public class BasicCredentials
{
    /// <summary>
    /// The username 
    /// </summary>
    public string Username { get; set; }
    
    /// <summary>
    /// The password
    /// </summary>
    public string Password { get; set; }

    /// <summary>
    /// The empty basic credentials
    /// </summary>
    public static readonly BasicCredentials EMPTY = new();
}