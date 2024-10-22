using Domain.Repositories;
using Infrastructure.Repositories;
using OzonHW2.Interceptors;
using OzonHW2.Services;
using System.Reflection;
using FluentValidation;
using Infrastructure;
using Microsoft.Extensions.Options;
using Grpc.Reflection;
using Infrastructure.Services;
using Microsoft.OpenApi.Models;
using OzonHW2.Validators;


namespace OzonHW2;

public class Startup
{
    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        
        services.AddGrpc(op=>{
            op.Interceptors.Add<LoggerInterceptor>();
            op.Interceptors.Add<ExceptionInterceptor>();
        });
        
        services.AddGrpcReflection();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        services.AddSingleton<IGoodItemRepository, GoodItemRepository>();
        services.AddScoped<IGoodItemService, GoodItemService>();
        services.AddValidatorsFromAssemblyContaining<FilterRequestValidator>();
        services.AddValidatorsFromAssemblyContaining<GoodsDtoValidator>();
        services.AddValidatorsFromAssemblyContaining<GoodsIdValidator>();
        services.AddValidatorsFromAssemblyContaining<UpdatePriceRequestValidator>();
    }


    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseRouting();
      
        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapGrpcService<GreeterService>();
            endpoints.MapGrpcReflectionService();
        });
    }

}