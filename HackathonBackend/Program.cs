namespace HackathonBackend;

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class Program
{
    public static void Main(string[] Args)
    {
        CreateHostBuilder(Args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] Args) =>
        Host.CreateDefaultBuilder(Args)
            .ConfigureWebHostDefaults(WebBuilder =>
            {
                WebBuilder.UseStartup<Startup>();
            });
}
