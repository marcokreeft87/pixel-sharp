using Microsoft.AspNetCore.Mvc;

public class MatrixController : Controller
{
    private readonly IPixelSharpMatrix _matrix;
    
    public MatrixController(IPixelSharpMatrix matrix) => _matrix = matrix;

    [HttpGet("matrix/render")]
    public void RenderImage([FromQuery] string imageUrl) => _matrix.ShowBitmapFromUrl(imageUrl);

    [HttpGet("matrix/text")]
    public void ScrollText([FromQuery] string text, CancellationToken cancellationToken) => _matrix.ScrollText(text, cancellationToken);  
}