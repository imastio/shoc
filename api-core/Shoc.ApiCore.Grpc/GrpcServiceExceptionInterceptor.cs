using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Grpc.Core.Interceptors;
using Shoc.Core;
using Shoc.Core.Grpc;
using Status = Google.Rpc.Status;

namespace Shoc.ApiCore.Grpc;

/// <summary>
/// The global interceptor for the exception handling
/// </summary>
public class GrpcServiceExceptionInterceptor : Interceptor
{
    /// <summary>
    /// Handle the unary server request
    /// </summary>
    /// <param name="request">The request</param>
    /// <param name="context">The context</param>
    /// <param name="continuation">The continuation</param>
    /// <typeparam name="TRequest">The type of request</typeparam>
    /// <typeparam name="TResponse">The type of response</typeparam>
    /// <returns></returns>
    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(TRequest request, ServerCallContext context, UnaryServerMethod<TRequest, TResponse> continuation)
    {
        try
        {
            return await continuation(request, context);
        }
        catch (Exception e)
        {
            throw ToRpcException(e);
        }
    }

    /// <summary>
    /// Handle the streaming server request
    /// </summary>
    /// <param name="requestStream">The request stream</param>
    /// <param name="context">The context</param>
    /// <param name="continuation">The continuation</param>
    /// <typeparam name="TRequest">The type of request</typeparam>
    /// <typeparam name="TResponse">The type of response</typeparam>
    /// <returns></returns>
    public override async Task<TResponse> ClientStreamingServerHandler<TRequest, TResponse>(IAsyncStreamReader<TRequest> requestStream, ServerCallContext context, ClientStreamingServerMethod<TRequest, TResponse> continuation)
    {
        try
        {
            return await continuation(requestStream, context);
        }
        catch (Exception e)
        {
            throw ToRpcException(e);
        }
    }

    /// <summary>
    /// Handle the streaming server request
    /// </summary>
    /// <param name="request">The request</param>
    /// <param name="responseStream">The response stream</param>
    /// <param name="context">The context</param>
    /// <param name="continuation">The continuation</param>
    /// <typeparam name="TRequest">The type of request</typeparam>
    /// <typeparam name="TResponse">The type of response</typeparam>
    /// <returns></returns>
    public override async Task ServerStreamingServerHandler<TRequest, TResponse>(TRequest request, IServerStreamWriter<TResponse> responseStream, ServerCallContext context, ServerStreamingServerMethod<TRequest, TResponse> continuation)
    {
        try
        {
            await continuation(request, responseStream, context);
        }
        catch (Exception e)
        {
            throw ToRpcException(e);
        }
    }

    /// <summary>
    /// Handle the streaming server request
    /// </summary>
    /// <param name="requestStream">The request</param>
    /// <param name="responseStream">The response stream</param>
    /// <param name="context">The context</param>
    /// <param name="continuation">The continuation</param>
    /// <typeparam name="TRequest">The type of request</typeparam>
    /// <typeparam name="TResponse">The type of response</typeparam>
    /// <returns></returns>
    public override async Task DuplexStreamingServerHandler<TRequest, TResponse>(IAsyncStreamReader<TRequest> requestStream, IServerStreamWriter<TResponse> responseStream, ServerCallContext context, DuplexStreamingServerMethod<TRequest, TResponse> continuation)
    {
        try
        {
            await continuation(requestStream, responseStream, context);
        }
        catch (Exception e)
        {
            throw ToRpcException(e);
        }
    }

    /// <summary>
    /// Rethrows the exception with proper RPC format
    /// </summary>
    /// <param name="exception">The exception</param>
    /// <returns></returns>
    private static RpcException ToRpcException(Exception exception)
    {
        // cast to specific exception
        var specificException = exception as ShocException;
        
        // unknown error
        var errors = new List<ErrorDefinition> { ErrorDefinition.Unknown(Errors.UNKNOWN_ERROR, exception.Message) };

        // if specific exception consider getting internal errors
        if (specificException?.Errors?.Count > 0)
        {
            errors = specificException.Errors;
        }
        
        // create status object
        var status = new Status
        {
            Code = (int)GrpcErrorMapping.ToStatusCode(errors[0].Kind),
            Message = errors[0].Message
        };
        
        // add errors as details
        status.Details.AddRange(errors.Select(error => Any.Pack(GrpcErrorMapping.ToGrpcErrorDefinition(error))));

        // rethrow the exception
        return status.ToRpcException();
    }
}