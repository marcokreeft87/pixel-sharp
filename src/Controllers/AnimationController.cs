using Microsoft.AspNetCore.Mvc;
using PixelSharp.Extensions;

namespace PixelSharp.Controllers;

public class AnimationController : Controller
{
    private readonly IPixelSharpMatrix _matrix;

    public AnimationController(IPixelSharpMatrix matrix) => _matrix = matrix;

    [HttpGet("clock")]
    public async Task RenderClock(CancellationToken cancellationToken)
    {
        await Task.Run(() =>
        {            
            var matrixCanvas = _matrix.CreateCanvas();
            while (!cancellationToken.IsCancellationRequested)
            {
                _matrix.DrawClock(matrixCanvas);
                
                // Wait for 1 second
                Thread.Sleep(1000);
            }
        }, cancellationToken);
    }

    [HttpGet("weather")]
    public async Task RenderWeather(CancellationToken cancellationToken)
    {
        await Task.Run(() =>
        {
            var matrixCanvas = _matrix.CreateCanvas();
            while (!cancellationToken.IsCancellationRequested)
            {
                _matrix.DrawWeather(matrixCanvas);
                
                // Wait for 1 hour
                Thread.Sleep(3600000);
            }
        }, cancellationToken);
    }
}