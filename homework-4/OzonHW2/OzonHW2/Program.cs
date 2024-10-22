using Domain.Repositories;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using OzonHW2.Services;

namespace OzonHW2;

public  class Program
{
    public static async Task Main(string[] args)
        {
            await Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.ConfigureKestrel(op=>{
                    op.ListenLocalhost(5001, o=> o.Protocols = HttpProtocols.Http2); 
                    op.ListenLocalhost(5115, o=> o.Protocols = HttpProtocols.Http1); 
                });
                webBuilder.UseStartup<Startup>();
            }).Build().RunAsync();
        }

    
}