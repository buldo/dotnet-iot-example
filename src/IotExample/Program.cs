using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace IotExample;

internal class Program
{
    static void Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);

        builder.Services
            .AddHostedService<Worker>()
            .AddSystemd();

        builder.Logging
            .AddSystemdConsole();

        var host = builder.Build();
        host.Run();
    }
}