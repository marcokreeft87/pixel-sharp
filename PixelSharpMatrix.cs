using rpi_rgb_led_matrix_sharp;
using SkiaSharp;

public class PixelSharpMatrix : IPixelSharpMatrix
{
    private int _ledRows;
    private int _ledColumns;
    private RGBLedMatrix _matrix { get; }

    public PixelSharpMatrix(IConfiguration configuration)
    {
        (_ledRows, _ledColumns) = GetDimensionsFromConfiguration(configuration);

        _matrix = GetMatrix(_ledRows, _ledColumns);
    }

    public RGBLedCanvas Render(RGBLedCanvas canvas) 
    {
        canvas = _matrix.SwapOnVsync(canvas);
        canvas.Clear();

        return canvas;
    }

    public void ScrollText(string text, CancellationToken cancellationToken)
    {
        text = text ?? "This is a scrolling text.";

        var canvas = _matrix.CreateOffscreenCanvas();
        var font = new RGBLedFont("fonts/6x10.bdf");

        var textLength = canvas.DrawText(font, 1, 6, new Color(0, 255, 0), text);
    
        canvas = _matrix.CreateOffscreenCanvas();

        while (!cancellationToken.IsCancellationRequested) {
            var x = canvas.Width;
            canvas.Clear();
            while (x > -textLength) {
                canvas.DrawText(font, x, 6, new Color(0, 255, 0), text);
                x -= 1;
                canvas = Render(canvas);
            }
        }
    }

    public void ShowLogo() => ShowBitmapFromPath("logo.png");

    public void ShowBitmapFromUrl(string imageUrl)
    {
        var bitmap = BitmapHelper.GetBitmapFromUrl(imageUrl, _ledRows, _ledColumns).Result;

        var canvas = _matrix.CreateOffscreenCanvas();
        canvas = DrawBitmapOnCanvas(canvas, bitmap);

        Render(canvas);
    }

    public void ShowBitmapFromPath(string path)
    {
        var canvas = _matrix.CreateOffscreenCanvas();

        var bitmap = BitmapHelper.GetBitmapFromPath(path, _ledRows, _ledColumns);

        canvas = DrawBitmapOnCanvas(canvas, bitmap);

        Render(canvas);
    }

    private RGBLedCanvas DrawBitmapOnCanvas(RGBLedCanvas canvas, SKBitmap bitmap)
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

        // // Swap the canvas on the next vertical sync to display the image
        // canvas = matrix.SwapOnVsync(canvas);

        return canvas;
    }


    private RGBLedMatrix GetMatrix(int rows, int columns) 
    {
        var options = new RGBLedMatrixOptions() 
        {
            Rows = rows,
            Cols = columns,
            ChainLength = 1,
            Parallel = 1,
            GpioSlowdown = 4,
            Brightness = 100,
            //DisableHardwarePulsing = true
        };

        return new RGBLedMatrix(options);        
    }

    private (int rows, int columns) GetDimensionsFromConfiguration(IConfiguration configuration)
    {
        var pixelDisplaySettings = configuration.GetSection("PixelDisplaySettings").Get<PixelDisplaySettings>();

        if(pixelDisplaySettings == null)
            throw new Exception("PixelDisplaySettings is null");

        return (pixelDisplaySettings.LedRows, pixelDisplaySettings.LedColumns);
    }
}