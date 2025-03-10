// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: workspace.workspaces/services.proto
// </auto-generated>
#pragma warning disable 0414, 1591, 8981, 0612
#region Designer generated code

using grpc = global::Grpc.Core;

namespace Shoc.Workspace.Grpc.Workspaces {
  public static partial class WorkspaceServiceGrpc
  {
    static readonly string __ServiceName = "api.grpc.workspace.workspaces.WorkspaceServiceGrpc";

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static void __Helper_SerializeMessage(global::Google.Protobuf.IMessage message, grpc::SerializationContext context)
    {
      #if !GRPC_DISABLE_PROTOBUF_BUFFER_SERIALIZATION
      if (message is global::Google.Protobuf.IBufferMessage)
      {
        context.SetPayloadLength(message.CalculateSize());
        global::Google.Protobuf.MessageExtensions.WriteTo(message, context.GetBufferWriter());
        context.Complete();
        return;
      }
      #endif
      context.Complete(global::Google.Protobuf.MessageExtensions.ToByteArray(message));
    }

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static class __Helper_MessageCache<T>
    {
      public static readonly bool IsBufferMessage = global::System.Reflection.IntrospectionExtensions.GetTypeInfo(typeof(global::Google.Protobuf.IBufferMessage)).IsAssignableFrom(typeof(T));
    }

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static T __Helper_DeserializeMessage<T>(grpc::DeserializationContext context, global::Google.Protobuf.MessageParser<T> parser) where T : global::Google.Protobuf.IMessage<T>
    {
      #if !GRPC_DISABLE_PROTOBUF_BUFFER_SERIALIZATION
      if (__Helper_MessageCache<T>.IsBufferMessage)
      {
        return parser.ParseFrom(context.PayloadAsReadOnlySequence());
      }
      #endif
      return parser.ParseFrom(context.PayloadAsNewBuffer());
    }

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::Shoc.Workspace.Grpc.Workspaces.GetWorkspaceByIdRequest> __Marshaller_api_grpc_workspace_workspaces_GetWorkspaceByIdRequest = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::Shoc.Workspace.Grpc.Workspaces.GetWorkspaceByIdRequest.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::Shoc.Workspace.Grpc.Workspaces.GetWorkspaceResponse> __Marshaller_api_grpc_workspace_workspaces_GetWorkspaceResponse = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::Shoc.Workspace.Grpc.Workspaces.GetWorkspaceResponse.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::Shoc.Workspace.Grpc.Workspaces.GetWorkspaceByNameRequest> __Marshaller_api_grpc_workspace_workspaces_GetWorkspaceByNameRequest = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::Shoc.Workspace.Grpc.Workspaces.GetWorkspaceByNameRequest.Parser));

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Method<global::Shoc.Workspace.Grpc.Workspaces.GetWorkspaceByIdRequest, global::Shoc.Workspace.Grpc.Workspaces.GetWorkspaceResponse> __Method_GetById = new grpc::Method<global::Shoc.Workspace.Grpc.Workspaces.GetWorkspaceByIdRequest, global::Shoc.Workspace.Grpc.Workspaces.GetWorkspaceResponse>(
        grpc::MethodType.Unary,
        __ServiceName,
        "GetById",
        __Marshaller_api_grpc_workspace_workspaces_GetWorkspaceByIdRequest,
        __Marshaller_api_grpc_workspace_workspaces_GetWorkspaceResponse);

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Method<global::Shoc.Workspace.Grpc.Workspaces.GetWorkspaceByNameRequest, global::Shoc.Workspace.Grpc.Workspaces.GetWorkspaceResponse> __Method_GetByName = new grpc::Method<global::Shoc.Workspace.Grpc.Workspaces.GetWorkspaceByNameRequest, global::Shoc.Workspace.Grpc.Workspaces.GetWorkspaceResponse>(
        grpc::MethodType.Unary,
        __ServiceName,
        "GetByName",
        __Marshaller_api_grpc_workspace_workspaces_GetWorkspaceByNameRequest,
        __Marshaller_api_grpc_workspace_workspaces_GetWorkspaceResponse);

    /// <summary>Service descriptor</summary>
    public static global::Google.Protobuf.Reflection.ServiceDescriptor Descriptor
    {
      get { return global::Shoc.Workspace.Grpc.Workspaces.ServicesReflection.Descriptor.Services[0]; }
    }

    /// <summary>Base class for server-side implementations of WorkspaceServiceGrpc</summary>
    [grpc::BindServiceMethod(typeof(WorkspaceServiceGrpc), "BindService")]
    public abstract partial class WorkspaceServiceGrpcBase
    {
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::System.Threading.Tasks.Task<global::Shoc.Workspace.Grpc.Workspaces.GetWorkspaceResponse> GetById(global::Shoc.Workspace.Grpc.Workspaces.GetWorkspaceByIdRequest request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::System.Threading.Tasks.Task<global::Shoc.Workspace.Grpc.Workspaces.GetWorkspaceResponse> GetByName(global::Shoc.Workspace.Grpc.Workspaces.GetWorkspaceByNameRequest request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

    }

    /// <summary>Client for WorkspaceServiceGrpc</summary>
    public partial class WorkspaceServiceGrpcClient : grpc::ClientBase<WorkspaceServiceGrpcClient>
    {
      /// <summary>Creates a new client for WorkspaceServiceGrpc</summary>
      /// <param name="channel">The channel to use to make remote calls.</param>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public WorkspaceServiceGrpcClient(grpc::ChannelBase channel) : base(channel)
      {
      }
      /// <summary>Creates a new client for WorkspaceServiceGrpc that uses a custom <c>CallInvoker</c>.</summary>
      /// <param name="callInvoker">The callInvoker to use to make remote calls.</param>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public WorkspaceServiceGrpcClient(grpc::CallInvoker callInvoker) : base(callInvoker)
      {
      }
      /// <summary>Protected parameterless constructor to allow creation of test doubles.</summary>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      protected WorkspaceServiceGrpcClient() : base()
      {
      }
      /// <summary>Protected constructor to allow creation of configured clients.</summary>
      /// <param name="configuration">The client configuration.</param>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      protected WorkspaceServiceGrpcClient(ClientBaseConfiguration configuration) : base(configuration)
      {
      }

      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::Shoc.Workspace.Grpc.Workspaces.GetWorkspaceResponse GetById(global::Shoc.Workspace.Grpc.Workspaces.GetWorkspaceByIdRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return GetById(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::Shoc.Workspace.Grpc.Workspaces.GetWorkspaceResponse GetById(global::Shoc.Workspace.Grpc.Workspaces.GetWorkspaceByIdRequest request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_GetById, null, options, request);
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncUnaryCall<global::Shoc.Workspace.Grpc.Workspaces.GetWorkspaceResponse> GetByIdAsync(global::Shoc.Workspace.Grpc.Workspaces.GetWorkspaceByIdRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return GetByIdAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncUnaryCall<global::Shoc.Workspace.Grpc.Workspaces.GetWorkspaceResponse> GetByIdAsync(global::Shoc.Workspace.Grpc.Workspaces.GetWorkspaceByIdRequest request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_GetById, null, options, request);
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::Shoc.Workspace.Grpc.Workspaces.GetWorkspaceResponse GetByName(global::Shoc.Workspace.Grpc.Workspaces.GetWorkspaceByNameRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return GetByName(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::Shoc.Workspace.Grpc.Workspaces.GetWorkspaceResponse GetByName(global::Shoc.Workspace.Grpc.Workspaces.GetWorkspaceByNameRequest request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_GetByName, null, options, request);
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncUnaryCall<global::Shoc.Workspace.Grpc.Workspaces.GetWorkspaceResponse> GetByNameAsync(global::Shoc.Workspace.Grpc.Workspaces.GetWorkspaceByNameRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return GetByNameAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncUnaryCall<global::Shoc.Workspace.Grpc.Workspaces.GetWorkspaceResponse> GetByNameAsync(global::Shoc.Workspace.Grpc.Workspaces.GetWorkspaceByNameRequest request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_GetByName, null, options, request);
      }
      /// <summary>Creates a new instance of client from given <c>ClientBaseConfiguration</c>.</summary>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      protected override WorkspaceServiceGrpcClient NewInstance(ClientBaseConfiguration configuration)
      {
        return new WorkspaceServiceGrpcClient(configuration);
      }
    }

    /// <summary>Creates service definition that can be registered with a server</summary>
    /// <param name="serviceImpl">An object implementing the server-side handling logic.</param>
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    public static grpc::ServerServiceDefinition BindService(WorkspaceServiceGrpcBase serviceImpl)
    {
      return grpc::ServerServiceDefinition.CreateBuilder()
          .AddMethod(__Method_GetById, serviceImpl.GetById)
          .AddMethod(__Method_GetByName, serviceImpl.GetByName).Build();
    }

    /// <summary>Register service method with a service binder with or without implementation. Useful when customizing the service binding logic.
    /// Note: this method is part of an experimental API that can change or be removed without any prior notice.</summary>
    /// <param name="serviceBinder">Service methods will be bound by calling <c>AddMethod</c> on this object.</param>
    /// <param name="serviceImpl">An object implementing the server-side handling logic.</param>
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    public static void BindService(grpc::ServiceBinderBase serviceBinder, WorkspaceServiceGrpcBase serviceImpl)
    {
      serviceBinder.AddMethod(__Method_GetById, serviceImpl == null ? null : new grpc::UnaryServerMethod<global::Shoc.Workspace.Grpc.Workspaces.GetWorkspaceByIdRequest, global::Shoc.Workspace.Grpc.Workspaces.GetWorkspaceResponse>(serviceImpl.GetById));
      serviceBinder.AddMethod(__Method_GetByName, serviceImpl == null ? null : new grpc::UnaryServerMethod<global::Shoc.Workspace.Grpc.Workspaces.GetWorkspaceByNameRequest, global::Shoc.Workspace.Grpc.Workspaces.GetWorkspaceResponse>(serviceImpl.GetByName));
    }

  }
  public static partial class WorkspaceMemberServiceGrpc
  {
    static readonly string __ServiceName = "api.grpc.workspace.workspaces.WorkspaceMemberServiceGrpc";

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static void __Helper_SerializeMessage(global::Google.Protobuf.IMessage message, grpc::SerializationContext context)
    {
      #if !GRPC_DISABLE_PROTOBUF_BUFFER_SERIALIZATION
      if (message is global::Google.Protobuf.IBufferMessage)
      {
        context.SetPayloadLength(message.CalculateSize());
        global::Google.Protobuf.MessageExtensions.WriteTo(message, context.GetBufferWriter());
        context.Complete();
        return;
      }
      #endif
      context.Complete(global::Google.Protobuf.MessageExtensions.ToByteArray(message));
    }

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static class __Helper_MessageCache<T>
    {
      public static readonly bool IsBufferMessage = global::System.Reflection.IntrospectionExtensions.GetTypeInfo(typeof(global::Google.Protobuf.IBufferMessage)).IsAssignableFrom(typeof(T));
    }

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static T __Helper_DeserializeMessage<T>(grpc::DeserializationContext context, global::Google.Protobuf.MessageParser<T> parser) where T : global::Google.Protobuf.IMessage<T>
    {
      #if !GRPC_DISABLE_PROTOBUF_BUFFER_SERIALIZATION
      if (__Helper_MessageCache<T>.IsBufferMessage)
      {
        return parser.ParseFrom(context.PayloadAsReadOnlySequence());
      }
      #endif
      return parser.ParseFrom(context.PayloadAsNewBuffer());
    }

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::Shoc.Workspace.Grpc.Workspaces.GetWorkspaceMemberByUserIdRequest> __Marshaller_api_grpc_workspace_workspaces_GetWorkspaceMemberByUserIdRequest = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::Shoc.Workspace.Grpc.Workspaces.GetWorkspaceMemberByUserIdRequest.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::Shoc.Workspace.Grpc.Workspaces.GetWorkspaceMemberResponse> __Marshaller_api_grpc_workspace_workspaces_GetWorkspaceMemberResponse = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::Shoc.Workspace.Grpc.Workspaces.GetWorkspaceMemberResponse.Parser));

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Method<global::Shoc.Workspace.Grpc.Workspaces.GetWorkspaceMemberByUserIdRequest, global::Shoc.Workspace.Grpc.Workspaces.GetWorkspaceMemberResponse> __Method_GetByUserId = new grpc::Method<global::Shoc.Workspace.Grpc.Workspaces.GetWorkspaceMemberByUserIdRequest, global::Shoc.Workspace.Grpc.Workspaces.GetWorkspaceMemberResponse>(
        grpc::MethodType.Unary,
        __ServiceName,
        "GetByUserId",
        __Marshaller_api_grpc_workspace_workspaces_GetWorkspaceMemberByUserIdRequest,
        __Marshaller_api_grpc_workspace_workspaces_GetWorkspaceMemberResponse);

    /// <summary>Service descriptor</summary>
    public static global::Google.Protobuf.Reflection.ServiceDescriptor Descriptor
    {
      get { return global::Shoc.Workspace.Grpc.Workspaces.ServicesReflection.Descriptor.Services[1]; }
    }

    /// <summary>Base class for server-side implementations of WorkspaceMemberServiceGrpc</summary>
    [grpc::BindServiceMethod(typeof(WorkspaceMemberServiceGrpc), "BindService")]
    public abstract partial class WorkspaceMemberServiceGrpcBase
    {
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::System.Threading.Tasks.Task<global::Shoc.Workspace.Grpc.Workspaces.GetWorkspaceMemberResponse> GetByUserId(global::Shoc.Workspace.Grpc.Workspaces.GetWorkspaceMemberByUserIdRequest request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

    }

    /// <summary>Client for WorkspaceMemberServiceGrpc</summary>
    public partial class WorkspaceMemberServiceGrpcClient : grpc::ClientBase<WorkspaceMemberServiceGrpcClient>
    {
      /// <summary>Creates a new client for WorkspaceMemberServiceGrpc</summary>
      /// <param name="channel">The channel to use to make remote calls.</param>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public WorkspaceMemberServiceGrpcClient(grpc::ChannelBase channel) : base(channel)
      {
      }
      /// <summary>Creates a new client for WorkspaceMemberServiceGrpc that uses a custom <c>CallInvoker</c>.</summary>
      /// <param name="callInvoker">The callInvoker to use to make remote calls.</param>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public WorkspaceMemberServiceGrpcClient(grpc::CallInvoker callInvoker) : base(callInvoker)
      {
      }
      /// <summary>Protected parameterless constructor to allow creation of test doubles.</summary>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      protected WorkspaceMemberServiceGrpcClient() : base()
      {
      }
      /// <summary>Protected constructor to allow creation of configured clients.</summary>
      /// <param name="configuration">The client configuration.</param>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      protected WorkspaceMemberServiceGrpcClient(ClientBaseConfiguration configuration) : base(configuration)
      {
      }

      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::Shoc.Workspace.Grpc.Workspaces.GetWorkspaceMemberResponse GetByUserId(global::Shoc.Workspace.Grpc.Workspaces.GetWorkspaceMemberByUserIdRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return GetByUserId(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::Shoc.Workspace.Grpc.Workspaces.GetWorkspaceMemberResponse GetByUserId(global::Shoc.Workspace.Grpc.Workspaces.GetWorkspaceMemberByUserIdRequest request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_GetByUserId, null, options, request);
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncUnaryCall<global::Shoc.Workspace.Grpc.Workspaces.GetWorkspaceMemberResponse> GetByUserIdAsync(global::Shoc.Workspace.Grpc.Workspaces.GetWorkspaceMemberByUserIdRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return GetByUserIdAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncUnaryCall<global::Shoc.Workspace.Grpc.Workspaces.GetWorkspaceMemberResponse> GetByUserIdAsync(global::Shoc.Workspace.Grpc.Workspaces.GetWorkspaceMemberByUserIdRequest request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_GetByUserId, null, options, request);
      }
      /// <summary>Creates a new instance of client from given <c>ClientBaseConfiguration</c>.</summary>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      protected override WorkspaceMemberServiceGrpcClient NewInstance(ClientBaseConfiguration configuration)
      {
        return new WorkspaceMemberServiceGrpcClient(configuration);
      }
    }

    /// <summary>Creates service definition that can be registered with a server</summary>
    /// <param name="serviceImpl">An object implementing the server-side handling logic.</param>
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    public static grpc::ServerServiceDefinition BindService(WorkspaceMemberServiceGrpcBase serviceImpl)
    {
      return grpc::ServerServiceDefinition.CreateBuilder()
          .AddMethod(__Method_GetByUserId, serviceImpl.GetByUserId).Build();
    }

    /// <summary>Register service method with a service binder with or without implementation. Useful when customizing the service binding logic.
    /// Note: this method is part of an experimental API that can change or be removed without any prior notice.</summary>
    /// <param name="serviceBinder">Service methods will be bound by calling <c>AddMethod</c> on this object.</param>
    /// <param name="serviceImpl">An object implementing the server-side handling logic.</param>
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    public static void BindService(grpc::ServiceBinderBase serviceBinder, WorkspaceMemberServiceGrpcBase serviceImpl)
    {
      serviceBinder.AddMethod(__Method_GetByUserId, serviceImpl == null ? null : new grpc::UnaryServerMethod<global::Shoc.Workspace.Grpc.Workspaces.GetWorkspaceMemberByUserIdRequest, global::Shoc.Workspace.Grpc.Workspaces.GetWorkspaceMemberResponse>(serviceImpl.GetByUserId));
    }

  }
}
#endregion
