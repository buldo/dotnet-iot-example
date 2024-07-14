using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace IotExample;

internal class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;

    public Worker(
        ILogger<Worker> logger,
        IHostApplicationLifetime appLifetime)
    {
        _logger = logger;
        appLifetime.ApplicationStopping.Register(OnStopping);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var timer = new PeriodicTimer(TimeSpan.FromSeconds(10));
        while (await timer.WaitForNextTickAsync(stoppingToken))
        {
            _logger.LogWarning("Timer tick");
        }
    }

    private void OnStopping()
    {
        _logger.LogWarning("Stopping");
    }
}