namespace Shoc.Identity.Provider.Config;

/// <summary>
/// The identity settings
/// </summary>
public class IdentitySettings
{
    /// <summary>
    /// The login url
    /// </summary>
    public string SignInUrl { get; set; }

    /// <summary>
    /// The login url
    /// </summary>
    public string SignOutUrl { get; set; }

    /// <summary>
    /// The error url
    /// </summary>
    public string ErrorUrl { get; set; }

    /// <summary>
    /// The machine to machine settings
    /// </summary>
    public MachineToMachineSettings MachineToMachine { get; set; }

    /// <summary>
    /// The interactive client settings
    /// </summary>
    public InteractiveClientSettings InteractiveClient { get; set; }
}
