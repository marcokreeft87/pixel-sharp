using Microsoft.AspNetCore.Mvc;
using rpi_rgb_led_matrix_sharp;
using Color = rpi_rgb_led_matrix_sharp.Color;

public class MatrixController : Controller
{
    private int _ledRows;
    private int _ledColumns;

    public MatrixController(IConfiguration configuration)
    {
        var pixelDisplaySettings = configuration.GetSection("PixelDisplaySettings").Get<PixelDisplaySettings>();

        if(pixelDisplaySettings == null)
            throw new Exception("PixelDisplaySettings is null");

        _ledRows = pixelDisplaySettings.LedRows;
        _ledColumns = pixelDisplaySettings.LedColumns;
    }

    [HttpGet("matrix/render")]
    public async Task<string> RenderImage([FromQuery] string imageUrl) 
    {
        // Initialize the LED matrix display
        var options = new RGBLedMatrixOptions() 
        {
            Rows = _ledRows,
            Cols = _ledColumns,
            ChainLength = 1,
            Parallel = 1,
            GpioSlowdown = 4,
        };

        var matrix = new RGBLedMatrix(options);
        var canvas = matrix.CreateOffscreenCanvas();
        canvas.Clear();
        
        var bitmap = await BitmapHelper.GetBitmap(imageUrl, _ledColumns, _ledRows);

        // Loop through the pixels of the image and set the corresponding pixel in the RGBLedMatrix canvas
        var xOffset = (canvas.Width - bitmap.Width) / 2;
        var yOffset = (canvas.Height - bitmap.Height) / 2;

        // Loop through the pixels of the image and set the corresponding pixel in the RGBLedMatrix canvas
        for (var y = 0; y < bitmap.Height; ++y)
        {
            for (var x = 0; x < bitmap.Width; ++x)
            {
                var pixel = bitmap.GetPixel(x, y);

                canvas.SetPixel(x + xOffset, y + yOffset, new Color(pixel.Red, pixel.Green, pixel.Blue));
            }
        }

        // Swap the canvas on the next vertical sync to display the image
        canvas = matrix.SwapOnVsync(canvas);
        
        return "Image rendered";
    }
}