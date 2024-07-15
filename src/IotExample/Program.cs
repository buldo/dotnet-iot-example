using Iot.Device.Board;
using Iot.Device.Graphics.SkiaSharpAdapter;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace IotExample;

internal class Program
{
    static void Main(string[] args)
    {
        SkiaSharpAdapter.Register();
        var builder = Host.CreateApplicationBuilder(args);

        builder.Services
            .AddHostedService<Worker>()
            .AddSingleton<Board>(Board.Create())
            .AddSystemd();

        builder.Logging
            .AddSystemdConsole();

        var host = builder.Build();
        host.Run();
    }
}