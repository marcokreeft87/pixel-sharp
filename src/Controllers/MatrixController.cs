using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

public class MatrixController : Controller
{
    private readonly IPixelSharpMatrix _matrix;
    
    public MatrixController(IPixelSharpMatrix matrix) => _matrix = matrix;

    [HttpGet("matrix/gif")]
    public void RenderGif([FromQuery] string imageUrl, CancellationToken cancellationToken) => _matrix.DrawGifFromUrl(imageUrl, cancellationToken);

    [HttpGet("matrix/image")]
    public void RenderImage([FromQuery] string imageUrl) => _matrix.DrawBitmapFromUrl(imageUrl);

    [HttpPost("matrix/image64")]
    public void RenderBase64([FromBody] string base64String) => _matrix.DrawBitmapFromBase64(base64String);

    [HttpPost("matrix/gif64")]
    public void RenderGifBase64([FromBody] string base64String, CancellationToken cancellationToken) => _matrix.DrawGifFromBase64(base64String, cancellationToken);
    
    [HttpGet("matrix/text")]
    public void RenderText([FromQuery] string text, CancellationToken cancellationToken) => _matrix.DrawText(text, cancellationToken);  

    [HttpGet("matrix/scroll")]
    public void ScrollText([FromQuery] string text, CancellationToken cancellationToken) => _matrix.ScrollText(text, cancellationToken);  

    [HttpPost("matrix/render")]
    public string Render([FromBody] RenderRequest request, CancellationToken cancellationToken) 
    {
        /*
        {
            "Width": 64,
            "Height": 64,
            "Sections": [
                {
                    "Start": {
                        "X": 1,
                        "Y": 10
                    },
                    "End": {
                        "X": 64,
                        "Y": 20
                    },
                    "Graphic": {
                        "Type": 0,
                        "Content": "Itsa me"
                    }
                },
                {
                    "Start": {
                        "X": 1,
                        "Y": 18
                    },
                    "End": {
                        "X": 64,
                        "Y": 18
                    },
                    "Graphic": {
                        "Type": 0,
                        "Content": "MARIO!!"
                    }
                },
                {
                    "Start": {
                        "X": 1,
                        "Y": 28
                    },
                    "End": {
                        "X": 32,
                        "Y": 64
                    },
                    "Graphic": {
                        "Type": 1,
                        "Content": "https://mario.wiki.gallery/images/8/89/MPS_Toad_Artwork.png"
                    }
                },
                {
                    "Start": {
                        "X": 33,
                        "Y": 28
                    },
                    "End": {
                        "X": 64,
                        "Y": 64
                    },
                    "Graphic": {
                        "Type": 1,
                        "Content": "https://mario.wiki.gallery/images/3/3e/MPSS_Mario.png"
                    }
                }
            ]
        }

        */

        // Check if request is the correct dimensions
        if (request.Width != _matrix.Width || request.Height != _matrix.Height)
        {
            // If not, return a 400 Bad Request
            Response.StatusCode = 400;
            Response.WriteAsync($"The request must be {_matrix.Width}x{_matrix.Height}.");
            return string.Empty;
        }

        // Render the request
        _matrix.Render(request, cancellationToken);
        return JsonConvert.SerializeObject(request);
    }
}