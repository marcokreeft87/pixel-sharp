using Microsoft.AspNetCore.Mvc;
using rpi_rgb_led_matrix_sharp;
using Color = rpi_rgb_led_matrix_sharp.Color;

public class MatrixController : Controller
{
    private int _ledRows;
    private int _ledColumns;

    public MatrixController(IConfiguration configuration)
    {
        (_ledRows, _ledColumns) = MatrixHelper.GetDimensionsFromConfiguration(configuration);
    }

    [HttpGet("matrix/render")]
    public async Task<string> RenderImage([FromQuery] string imageUrl)
    {
        var matrix = MatrixHelper.GetMatrix(_ledRows, _ledColumns);
        matrix.GetCanvas().Clear();

        var canvas = matrix.CreateOffscreenCanvas();

        var bitmap = await BitmapHelper.GetBitmapFromUrl(imageUrl, _ledColumns, _ledRows);
        canvas = MatrixHelper.DrawBitmapOnCanvas(matrix, canvas, bitmap);

        return "Image rendered";
    }

    [HttpGet("matrix/text")]
    public void ScrollText([FromQuery] string text) 
    {
        var matrix = MatrixHelper.GetMatrix(_ledRows, _ledColumns);
        
        var canvas = matrix.CreateOffscreenCanvas();
        matrix.SwapOnVsync(canvas);

        text = "This is a scrolling text.";

        canvas = matrix.CreateOffscreenCanvas();
        var font = new RGBLedFont("/home/sharevalue/pixelsharp/bin/Debug/net7.0/fonts/6x10.bdf");

        canvas.DrawText(font, 1, 6, new Color(0, 255, 0), text);
        matrix.SwapOnVsync(canvas);
        

        // var canvas = matrix.CreateOffscreenCanvas();

        // var font = new RGBLedFont("fonts/6x10.bdf");

        // canvas.DrawText(font, 1, 6, new Color(0, 255, 0), text);
        
        // matrix.SwapOnVsync(canvas);
        // var graphics = new rpi_rgb_led_matrix_sharp.Graphics(matrix);
        // var font = new Font("Arial", 16, FontStyle.Bold);
        // var brush = new SolidBrush(Color.Red);


        // while (true)
        // {
        //     graphics.Clear();
        //     graphics.DrawText(text, font, brush, 0, 0, true);
        //     matrix.SwapOnVSync();
        // }
    }        
}