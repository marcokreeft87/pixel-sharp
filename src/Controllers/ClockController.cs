using Microsoft.AspNetCore.Mvc;
using PixelSharp.Extensions;

namespace PixelSharp.Controllers;

public class ClockController : Controller
{
    private readonly IPixelSharpMatrix _matrix;
    private CancellationToken _cancellationToken;

    public ClockController(IPixelSharpMatrix matrix) => _matrix = matrix;

    [HttpGet("clock")]
    public async Task RenderClock(CancellationToken cancellationToken)
    {
        _cancellationToken = cancellationToken;
        
        await Task.Run(() =>
        {            
            var matrixCanvas = _matrix.CreateCanvas();
            while (!_cancellationToken.IsCancellationRequested)
            {
                _matrix.DrawClock(matrixCanvas);
                
                // Wait for 1 second
                Thread.Sleep(1000);
            }
        }, cancellationToken);
    }
}