using Microsoft.AspNetCore.Mvc;
using PixelSharp.Extensions;
using rpi_rgb_led_matrix_sharp;

namespace PixelSharp.Controllers;

public class ClockController : Controller
{
    private RGBLedCanvas _canvas;
    private readonly IPixelSharpMatrix _matrix;
    private CancellationToken _cancellationToken;

    public ClockController(IPixelSharpMatrix matrix) => _matrix = matrix;

    [HttpGet("clock")]
    public void RenderClock(CancellationToken cancellationToken)
    {
        _canvas = _matrix.DrawClock();
        _cancellationToken = cancellationToken;

        new Timer(TimerTick, null, Timeout.Infinite, 1000); // Update every second
    }

    private void TimerTick(object? state)
    {
        if (_cancellationToken.IsCancellationRequested)
        {
            return;
        }

        _canvas = _matrix.DrawHands(_canvas);

        _canvas = _matrix.SwapCanvas(_canvas);
    }
}