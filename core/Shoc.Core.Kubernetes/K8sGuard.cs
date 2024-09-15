using System;
using System.Threading.Tasks;

namespace Shoc.Core.Kubernetes;

/// <summary>
/// The kubernetes API Guard
/// </summary>
public static class K8sGuard
{
    /// <summary>
    /// Do an operation within an exception guard async
    /// </summary>
    /// <typeparam name="T">The result type</typeparam>
    /// <param name="supplier">The supplier to execute</param>
    /// <returns></returns>
    public static async Task<T> DoAsync<T>(Func<Task<T>> supplier)
    {
        try
        {
            return await supplier.Invoke();
        }
        catch (ShocException)
        {
            throw;
        }
        catch (Exception e)
        {
            throw new ShocException([ErrorDefinition.Unknown(e.Message)], e.Message, e);
        }
    }
}