using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Shoc.Core;
using Shoc.Job.Model;

namespace Shoc.Job.Services;

/// <summary>
/// The resource parser service
/// </summary>
public class ResourceParser
{
    /// <summary>
    /// The valid memory value pattern
    /// </summary>
    private static readonly Regex MEMORY_PATTERN = new (@"^([\d.]+)([KMGTP]?i?B?)$", RegexOptions.IgnoreCase | RegexOptions.Compiled);
    
    /// <summary>
    /// The values of binary multipliers
    /// </summary>
    private static readonly Dictionary<string, long> BINARY_MULTIPLIERS = new()
    {
        { "Ki", 1024L },
        { "Mi", 1024L * 1024 },
        { "Gi", 1024L * 1024 * 1024 },
        { "Ti", 1024L * 1024 * 1024 * 1024 },
        { "Pi", 1024L * 1024 * 1024 * 1024 * 1024 },
        { "Ei", 1024L * 1024 * 1024 * 1024 * 1024 * 1024 }
    };

    /// <summary>
    /// The values of decimal multipliers
    /// </summary>
    private static readonly Dictionary<string, long> DECIMAL_MULTIPLIERS = new()
    {
        { "K", 1000L },
        { "M", 1000L * 1000 },
        { "G", 1000L * 1000 * 1000 },
        { "T", 1000L * 1000 * 1000 * 1000 },
        { "P", 1000L * 1000 * 1000 * 1000 * 1000 },
        { "E", 1000L * 1000 * 1000 * 1000 * 1000 * 1000 }
    };
    
    /// <summary>
    /// Parse the given value into millicores
    /// </summary>
    /// <param name="cpu">The CPU value to parse</param>
    /// <returns></returns>
    public long? ParseToMillicores(string cpu)
    {
        // empty string is valid, nothing is requested
        if (string.IsNullOrWhiteSpace(cpu))
        {
            return null;
        }

        // if value is just a number try parse and interpret as cores
        if (long.TryParse(cpu, out var cores))
        {
            return cores * 1000;
        }

        // if value is given as a floating point number try parse as cores
        if (double.TryParse(cpu, out var fractionalCores))
        {
            return (long)Math.Round(fractionalCores * 1000);
        }

        // if value is given in millicores handle separately
        if (cpu.EndsWith("m", StringComparison.OrdinalIgnoreCase))
        {
            // Input ends with 'm', parse as millicores
            if (long.TryParse(cpu[..^1], out var millicores))
            {
                return millicores;
            }
        }
        
        // could not be successfully parsed
        throw ErrorDefinition.Validation(JobErrors.INVALID_JOB_RESOURCES, $"The {cpu} is not valid value for CPU").AsException();
    }
    
    /// <summary>
    /// Parse the given memory into bytes
    /// </summary>
    /// <param name="memory">The memory value to parse</param>
    /// <returns></returns>
    public long? ParseToBytes(string memory)
    {
        // empty value is valid, no request
        if (string.IsNullOrWhiteSpace(memory))
        {
            return null;
        }
        
        // check if memory is already given in bytes
        if (long.TryParse(memory, out var bytes))
        {
            return bytes;
        }

        // match value to the valid pattern
        var match = MEMORY_PATTERN.Match(memory);

        // take the value from matcher
        var value = double.Parse(match.Groups[1].Value);
        
        // take the suffix from matcher
        var suffix = match.Groups[2].Value;

        // if multiplier is binary use the value and multiply
        if (BINARY_MULTIPLIERS.TryGetValue(suffix, out var binaryMultiplier))
        {
            return (long)Math.Round(value * binaryMultiplier);
        }
        
        // if the multiplier is decimal use the value and multiply
        if (DECIMAL_MULTIPLIERS.TryGetValue(suffix, out var decimalMultiplier))
        {
            return (long)Math.Round(value * decimalMultiplier);
        }

        // no suffix was found just interpret as bytes
        return (long)Math.Round(value);
    }
    
    /// <summary>
    /// Parse the 
    /// </summary>
    /// <param name="gpu"></param>
    /// <returns></returns>
    public long? ParseToGpu(string gpu)
    {
        // empty value is valid, no request
        if (string.IsNullOrWhiteSpace(gpu))
        {
            return null;
        }
        
        // try parse the value as is
        if (long.TryParse(gpu, out var gpuUnits))
        {
            return gpuUnits;
        }

        // the value is not valid to parse
        throw ErrorDefinition.Validation(JobErrors.INVALID_JOB_RESOURCES, $"The {gpu} is not valid value for GPU").AsException();
    }
}
