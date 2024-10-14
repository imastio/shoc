using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Shoc.Package.Model.Command;

namespace Shoc.Package.Services;

/// <summary>
/// The command runner service
/// </summary>
public class CommandRunner
{
    /// <summary>
    /// Runs the given command in a separate process
    /// </summary>
    /// <param name="command">The command to run</param>
    /// <returns></returns>
    public virtual async Task<CommandRunResult> RunBash(string command)
    {
        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "/bin/sh",
                Arguments = command,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
            }
        };
        
        try
        {
            // start the process
            process.Start();
            
            // read output
            var output = await process.StandardOutput.ReadToEndAsync();
            
            // read error
            var error = await process.StandardError.ReadToEndAsync();
            
            // wait for completion
            await process.WaitForExitAsync();

            // return result
            return new CommandRunResult
            {
                Success = process.ExitCode == 0,
                Output = output,
                Error = error
            };
        }
        catch (Exception ex)
        {
            return new CommandRunResult
            {
                Success = false,
                Output = string.Empty,
                Error = ex.Message
            };
        }
    }
}