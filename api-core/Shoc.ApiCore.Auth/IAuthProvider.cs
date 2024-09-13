using System;
using System.Threading.Tasks;

namespace Shoc.ApiCore.Auth;

/// <summary>
/// The authentication provider interface
/// </summary>
public interface IAuthProvider
{
    /// <summary>
    /// Executes an operation with token supplied
    /// </summary>
    /// <typeparam name="T">The type of return value</typeparam>
    /// <param name="securedFunction">The secured operation to execute</param>
    /// <returns></returns>
    public Task<T> DoAuthorized<T>(Func<string, Task<T>> securedFunction);

    /// <summary>
    /// Executes an operation with token supplied
    /// </summary>
    /// <typeparam name="T">The type of return value</typeparam>
    /// <param name="securedFunction">The secured operation to execute</param>
    /// <param name="scopes">The scopes to authorize with</param>
    /// <returns></returns>
    public Task<T> DoAuthorized<T>(Func<string, Task<T>> securedFunction, string[] scopes);
}
