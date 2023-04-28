using rpi_rgb_led_matrix_sharp;
using SkiaSharp;
using Color = rpi_rgb_led_matrix_sharp.Color;

public static class MatrixHelper 
{
    public static (int rows, int columns) GetDimensionsFromConfiguration(IConfiguration configuration)
    {
        var pixelDisplaySettings = configuration.GetSection("PixelDisplaySettings").Get<PixelDisplaySettings>();

        if(pixelDisplaySettings == null)
            throw new Exception("PixelDisplaySettings is null");

        return (pixelDisplaySettings.LedRows, pixelDisplaySettings.LedColumns);
    }

    public static RGBLedCanvas DrawBitmapOnCanvas(RGBLedMatrix matrix, RGBLedCanvas canvas, SKBitmap bitmap)
    {
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

        return canvas;
    }

    public static RGBLedMatrix GetMatrix(int rows, int columns)
    {
        // Initialize the LED matrix display
        var options = new RGBLedMatrixOptions() 
        {
            Rows = rows,
            Cols = columns,
            ChainLength = 1,
            Parallel = 1,
            GpioSlowdown = 4,
            Brightness = 100,
            DropPrivileges = false
        };

        return new RGBLedMatrix(options);        
    }

    public static void ShowLogo(int rows, int columns)
    {
        var matrix = MatrixHelper.GetMatrix(rows, columns);
        var canvas = matrix.CreateOffscreenCanvas();

        var bitmap = BitmapHelper.GetBitmapFromPath("logo.png", rows, columns);

        canvas = MatrixHelper.DrawBitmapOnCanvas(matrix, canvas, bitmap);
    }    
}