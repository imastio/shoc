using System;
using System.Security.Cryptography;
using System.Text;

namespace Shoc.Core;

/// <summary>
/// The random utilities
/// </summary>
public class Rnd
{
    /// <summary>
    /// All alphanumeric characters
    /// </summary>
    private static readonly char[] ALL_CHARS = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray(); 

    /// <summary>
    /// Generates a unique key of given size
    /// </summary>
    /// <param name="size">The size</param>
    /// <returns></returns>
    public static string GetString(int size)
    {            
        // result data 
        var data = new byte[4*size];
        
        // use crypto-random generator and fill bytes
        using (var crypto = RandomNumberGenerator.Create())
        {
            crypto.GetBytes(data);
        }
        
        // will fill string with random chars
        var result = new StringBuilder(size);
        
        // generate every char
        for (var i = 0; i < size; i++)
        {
            // get next random number from array
            var rnd = BitConverter.ToUInt32(data, i * 4);
            
            // pick the random id for character
            var idx = rnd % ALL_CHARS.Length;

            // add next char
            result.Append(ALL_CHARS[idx]);
        }

        return result.ToString();
    }
}
