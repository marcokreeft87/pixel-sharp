using Microsoft.AspNetCore.Mvc;

public class MatrixController : Controller
{
    private readonly IPixelSharpMatrix _matrix;
    
    public MatrixController(IPixelSharpMatrix matrix) => _matrix = matrix;

    [HttpGet("matrix/gif")]
    public void RenderGif([FromQuery] string imageUrl, CancellationToken cancellationToken) => _matrix.DrawGifFromUrl(imageUrl, cancellationToken);

    [HttpGet("matrix/image")]
    public void RenderImage([FromQuery] string imageUrl) => _matrix.DrawBitmapFromUrl(imageUrl);
    
    [HttpGet("matrix/text")]
    public void RenderText([FromQuery] string text, CancellationToken cancellationToken) => _matrix.DrawText(text, cancellationToken);  

    [HttpGet("matrix/scroll")]
    public void ScrollText([FromQuery] string text, CancellationToken cancellationToken) => _matrix.ScrollText(text, cancellationToken);  

    [HttpPost("matrix/render")]
    public void Render([FromBody] RenderRequest request, CancellationToken cancellationToken) 
    {
        // Check if request is the correct dimensions
        if (request.Width != _matrix.Width || request.Height != _matrix.Height)
        {
            // If not, return a 400 Bad Request
            Response.StatusCode = 400;
            Response.WriteAsync($"The request must be {_matrix.Width}x{_matrix.Height}.");
            return;
        }

        // Render the request
        _matrix.Render(request, cancellationToken);
    }
}