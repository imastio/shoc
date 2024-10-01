using System.CommandLine;

namespace Shoc.Cli.Commands.Auth
{
    /// <summary>
    /// The command to sign-in the user
    /// </summary>
    public class AuthSigninCommand : Command
    {
        /// <summary>
        /// Creates new instance of user sign-in command
        /// </summary>
        public AuthSigninCommand() : base("sign-in", "Signs in the user")
        {
            this.AddOption(new Option<bool>(new []{ "--silent" }, () => false, "Do sing-in silently without interaction"));
        }
    }
}