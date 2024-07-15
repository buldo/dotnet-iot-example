using System.Device.Gpio;
using System.Drawing;

using Iot.Device.Board;
using Iot.Device.Graphics;
using Iot.Device.Graphics.SkiaSharpAdapter;
using Iot.Device.Ssd13xx;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace IotExample;

internal class Worker : BackgroundService
{
    private const int PinNum = 20; // PA20
    private readonly ILogger<Worker> _logger;
    private readonly GpioController _pinController;
    private readonly GpioPin _pin;
    private readonly Ssd1306 _display;

    public Worker(
        ILogger<Worker> logger,
        Board board)
    {
        _logger = logger;
        _pinController = board.CreateGpioController();
        _pin = _pinController.OpenPin(PinNum, PinMode.Output);
        var i2cBus = board.CreateOrGetI2cBus(0, [11, 12]);
        var device = i2cBus.CreateDevice(0x3c);
        _display = new Ssd1306(device, 128, 32);
        _display.EnableDisplay(true);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var fontSize = 25;
        var font = "DejaVu Sans";
        var drawPoint = new Point(0, 0);

        var timer = new PeriodicTimer(TimeSpan.FromMilliseconds(100));
        while (await timer.WaitForNextTickAsync(stoppingToken))
        {
            _pin.Toggle();
            _logger.LogWarning("Timer tick");

            using var image = BitmapImage.CreateBitmap(128, 32, PixelFormat.Format32bppArgb);
            image.Clear(Color.Black);
            var drawingApi = image.GetDrawingApi();
            drawingApi.DrawText(DateTime.Now.ToString("HH:mm:ss"), font, fontSize, Color.White, drawPoint);
            _display.DrawBitmap(image);
        }
    }
}