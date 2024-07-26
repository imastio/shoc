using System.Collections.Generic;
using System.Threading.Tasks;
using Grpc.Core;
using Grpc.Core.Interceptors;
using Shoc.Core;
using Shoc.Core.Grpc;

namespace Shoc.ApiCore.GrpcClient;

/// <summary>
/// The global interceptor for the exception handling on the client side
/// </summary>
public class GrpcClientExceptionInterceptor : Interceptor
{
    /// <summary>
    /// Blocking unary call handler
    /// </summary>
    /// <param name="request">The request object</param>
    /// <param name="context">The request context</param>
    /// <param name="continuation">The continuation instance</param>
    /// <typeparam name="TRequest">The request type</typeparam>
    /// <typeparam name="TResponse">The response type</typeparam>
    /// <returns></returns>
    public override TResponse BlockingUnaryCall<TRequest, TResponse>(TRequest request, ClientInterceptorContext<TRequest, TResponse> context,
        BlockingUnaryCallContinuation<TRequest, TResponse> continuation)
    {
        try
        {
            return continuation(request, context);
        }
        catch (RpcException exception)
        {
            throw FromRpcException(exception);
        }
    }

    /// <summary>
    /// Async unary call handler
    /// </summary>
    /// <param name="request">The request object</param>
    /// <param name="context">The request context</param>
    /// <param name="continuation">The continuation instance</param>
    /// <typeparam name="TRequest">The request type</typeparam>
    /// <typeparam name="TResponse">The response type</typeparam>
    /// <returns></returns>
    public override AsyncUnaryCall<TResponse> AsyncUnaryCall<TRequest, TResponse>(TRequest request, ClientInterceptorContext<TRequest, TResponse> context,
        AsyncUnaryCallContinuation<TRequest, TResponse> continuation)
    {
        try
        {
            var call = continuation(request, context);

            return new AsyncUnaryCall<TResponse>(HandleResponse(call.ResponseAsync), call.ResponseHeadersAsync, call.GetStatus, call.GetTrailers, call.Dispose);
        }
        catch (RpcException exception)
        {
            throw FromRpcException(exception);
        }
    }

    /// <summary>
    /// Async server streaming call handler
    /// </summary>
    /// <param name="request">The request object</param>
    /// <param name="context">The request context</param>
    /// <param name="continuation">The continuation instance</param>
    /// <typeparam name="TRequest">The request type</typeparam>
    /// <typeparam name="TResponse">The response type</typeparam>
    /// <returns></returns>
    public override AsyncServerStreamingCall<TResponse> AsyncServerStreamingCall<TRequest, TResponse>(TRequest request,
        ClientInterceptorContext<TRequest, TResponse> context, AsyncServerStreamingCallContinuation<TRequest, TResponse> continuation)
    {
        try
        {
            return continuation(request, context);
        }
        catch (RpcException exception)
        {
            throw FromRpcException(exception);
        }
    }

    /// <summary>
    /// Async client streaming call handler
    /// </summary>
    /// <param name="context">The request context</param>
    /// <param name="continuation">The continuation instance</param>
    /// <typeparam name="TRequest">The request type</typeparam>
    /// <typeparam name="TResponse">The response type</typeparam>
    /// <returns></returns>
    public override AsyncClientStreamingCall<TRequest, TResponse> AsyncClientStreamingCall<TRequest, TResponse>(ClientInterceptorContext<TRequest, TResponse> context,
        AsyncClientStreamingCallContinuation<TRequest, TResponse> continuation)
    {
        try
        {
            return continuation(context);
        }
        catch (RpcException exception)
        {
            throw FromRpcException(exception);
        }
    }

    /// <summary>
    /// Async duplex streaming call handler
    /// </summary>
    /// <param name="context">The request context</param>
    /// <param name="continuation">The continuation instance</param>
    /// <typeparam name="TRequest">The request type</typeparam>
    /// <typeparam name="TResponse">The response type</typeparam>
    /// <returns></returns>
    public override AsyncDuplexStreamingCall<TRequest, TResponse> AsyncDuplexStreamingCall<TRequest, TResponse>(ClientInterceptorContext<TRequest, TResponse> context,
        AsyncDuplexStreamingCallContinuation<TRequest, TResponse> continuation)
    {
        try
        {
            return continuation(context);
        }
        catch (RpcException exception)
        {
            throw FromRpcException(exception);
        }
    }

    /// <summary>
    /// Handles the invokation in a safe context
    /// </summary>
    /// <param name="invoke">The invokation task</param>
    /// <typeparam name="TResponse">The response type</typeparam>
    /// <returns></returns>
    private static async Task<TResponse> HandleResponse<TResponse>(Task<TResponse> invoke)
    {
        try
        {
            return await invoke;
        }
        catch (RpcException exception)
        {
            throw FromRpcException(exception);
        }
    }
    
    /// <summary>
    /// Rethrows the exception with proper RPC format
    /// </summary>
    /// <param name="exception">The exception</param>
    /// <returns></returns>
    public static ShocException FromRpcException(RpcException exception)
    {
        // get primary error kind
        var kind = GrpcErrorMapping.ToErrorKind(exception.Status.StatusCode);

        // get RPC status
        var rpcStatus = exception.GetRpcStatus();

        // no RPC status known
        if (rpcStatus == null)
        {
            return ErrorDefinition.From(kind, Errors.UNKNOWN_ERROR, exception.Message).AsException();
        }

        // errors
        var errors = new List<ErrorDefinition>();

        // try process details
        foreach (var detail in rpcStatus.Details)
        {
            // try unpack grpc error
            var grpcError = detail.TryUnpack<GrpcErrorDefinition>(out var unpacked) ? unpacked : null;

            // skip if not valid
            if (grpcError == null)
            {
                continue;
            }
            
            // add to final collection
            errors.Add(GrpcErrorMapping.FromGrpcErrorDefinition(grpcError));
        }

        // no error
        return errors.Count == 0 ? ErrorDefinition.From(kind, Errors.UNKNOWN_ERROR, exception.Message).AsException() : new ShocException(errors);
    }

}