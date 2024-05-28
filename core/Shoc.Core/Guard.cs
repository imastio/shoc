using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shoc.Core;

/// <summary>
/// A guard for wrapping executions
/// </summary>
public static class Guard
{
    /// <summary>
    /// Do an action within an exception guard
    /// </summary>
    /// <param name="action">The action to execute</param>
    /// <returns></returns>
    public static void Do(Action action)
    {
        try
        {
            action.Invoke();
        }
        catch (ShocException)
        {
            throw;
        }
        catch (Exception e)
        {
            throw new ShocException(new List<ErrorDefinition>{ ErrorDefinition.Unknown(e.Message) }, e.Message, e);
        }
    }

    /// <summary>
    /// Do an operation within an exception guard
    /// </summary>
    /// <typeparam name="T">The result type</typeparam>
    /// <param name="supplier">The supplier to execute</param>
    /// <returns></returns>
    public static T Do<T>(Func<T> supplier)
    {
        try
        {
            return supplier.Invoke();
        }
        catch (ShocException)
        {
            throw;
        }
        catch (Exception e)
        {
            throw new ShocException(new List<ErrorDefinition> { ErrorDefinition.Unknown(e.Message) }, e.Message, e);
        }
    }


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
            throw new ShocException(new List<ErrorDefinition> { ErrorDefinition.Unknown(e.Message) }, e.Message, e);
        }
    }
}
