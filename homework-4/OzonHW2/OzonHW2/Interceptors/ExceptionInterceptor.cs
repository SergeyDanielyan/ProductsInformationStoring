﻿using Grpc.Core;
using Grpc.Core.Interceptors;

namespace OzonHW2.Interceptors;

public class ExceptionInterceptor : Interceptor
{
    private readonly ILogger<ExceptionInterceptor> _logger;

    public ExceptionInterceptor(ILogger<ExceptionInterceptor> logger)
    {
        _logger = logger;
    }

    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(TRequest request, ServerCallContext context, UnaryServerMethod<TRequest, TResponse> continuation)
    {
        try{
            return await continuation(request, context);
        }
        catch(Exception exc)
        {
            _logger.LogError(exc, "problem");

            throw new RpcException(new Status(StatusCode.NotFound, exc.Message));
        }
    }

}