using Newtonsoft.Json;
using rpi_rgb_led_matrix_sharp;
using SkiaSharp;

public class PixelSharpMatrix : IPixelSharpMatrix
{
    private int _ledRows;
    private int _ledColumns;
    private RGBLedMatrix _matrix { get; }

    public int Width => _ledRows;
    public int Height => _ledColumns;

    public PixelSharpMatrix(IConfiguration configuration)
    {
        (_ledRows, _ledColumns) = GetDimensionsFromConfiguration(configuration);

        _matrix = GetMatrix(_ledRows, _ledColumns);
    }

    public void Render(RenderRequest request, CancellationToken cancellationToken)
    {
        var canvas = _matrix.CreateOffscreenCanvas();

        if(request.Sections == null || request.Sections.Length == 0)
        {
            throw new ArgumentException("The request must have at least one section.");
        }

        foreach(var section in request.Sections)
        {
            Console.WriteLine($"Section: {JsonConvert.SerializeObject(section)}");

            var graphic = section.Graphic;
            if((graphic == null || (string.IsNullOrWhiteSpace(graphic.Content) && graphic.Pixels == null)))
            {
                throw new ArgumentException("The section must have a graphic and content or have pixels.");
            }

            switch(graphic.Type)
            {
                case GraphicType.Pixels:                
                    canvas = DrawPixelsOnCanvas(canvas, graphic.Pixels, section.Start, section.End);
                    break;
                case GraphicType.Text:                
                    canvas = DrawTextOnCanvas(canvas, graphic.Content, section.Start, section.End);
                    break;
                case GraphicType.Image:
                    canvas = DrawBitmapOnCanvas(canvas, graphic.Content, section.Start, section.End);
                    break;
                case GraphicType.Gif:
                    throw new NotImplementedException();
            }
        }

        canvas = SwapCanvas(canvas);
    }

    private RGBLedCanvas DrawPixelsOnCanvas(RGBLedCanvas canvas, RenderPixel[]? pixels, RenderPoint? start, RenderPoint? end)
    {
        if(pixels == null || pixels.Length == 0)
        {
            throw new ArgumentException("The section must have at least one pixel.");
        }

        foreach(var pixel in pixels)
        {
            canvas.SetPixel(pixel.X, pixel.Y, new Color(pixel.R, pixel.G, pixel.B));
        }

        return canvas;
    }

    public void DrawText(string text, CancellationToken cancellationToken)
    {
        text = text ?? "This is a static text.";

        var canvas = _matrix.CreateOffscreenCanvas();
        var font = new RGBLedFont("fonts/6x10.bdf");

        canvas = DrawTextOnCanvas(canvas, text, new RenderPoint(1, 6), null);

        canvas = SwapCanvas(canvas);
    }
    public void ScrollText(string text, CancellationToken cancellationToken)
    {
        text = text ?? "This is a scrolling text.";

        var canvas = _matrix.CreateOffscreenCanvas();
        var font = new RGBLedFont("fonts/6x10.bdf");

        var textLength = canvas.DrawText(font, 1, 6, new Color(0, 255, 0), text);

        canvas = _matrix.CreateOffscreenCanvas();

        int animationCount = 0;

        while (animationCount < 5 && !cancellationToken.IsCancellationRequested)
        {
            var x = canvas.Width;
            canvas.Clear();

            while (x > -textLength) {
                canvas.DrawText(font, x, 20, new Color(0, 255, 0), text);
                x -= 1;
                canvas = SwapCanvas(canvas);

                Thread.Sleep(50);
            }

            animationCount++;
            x = canvas.Width;
        }
    }

    public void DrawLogo() => DrawBitmapFromPath("logo.png");

    public void DrawGifFromUrl(string imageUrl, CancellationToken cancellationToken)
    {
        var canvas = _matrix.CreateOffscreenCanvas();

        (List<SKBitmap> frames, List<int> durations) = GraphicsHelper.GetGifFromUrl(imageUrl, _ledRows, _ledColumns);

        while(!cancellationToken.IsCancellationRequested)
        {
            for(var i = 0; i < frames.Count; i++)
            {
                canvas = DrawBitmapOnCanvas(canvas, frames[i]);
                canvas = SwapCanvas(canvas);

                Thread.Sleep(durations[i]);
            }

            // TODO remove frame skip after last frame
        }
    }

    public void DrawBitmapFromUrl(string imageUrl)
    {
        var bitmap = GraphicsHelper.GetBitmapFromUrl(imageUrl, _ledRows, _ledColumns).Result;

        var canvas = _matrix.CreateOffscreenCanvas();
        canvas = DrawBitmapOnCanvas(canvas, bitmap);

        SwapCanvas(canvas);
    }

    public void DrawBitmapFromPath(string path)
    {
        var canvas = _matrix.CreateOffscreenCanvas();

        var bitmap = GraphicsHelper.GetBitmapFromPath(path, _ledRows, _ledColumns);

        canvas = DrawBitmapOnCanvas(canvas, bitmap);

        SwapCanvas(canvas);
    }

    public RGBLedCanvas SwapCanvas(RGBLedCanvas canvas) 
    {
        canvas = _matrix.SwapOnVsync(canvas);
        canvas.Clear();

        return canvas;
    }
    
    private RGBLedCanvas DrawBitmapOnCanvas(RGBLedCanvas canvas, string imageUrl, RenderPoint start, RenderPoint end)
    {
        var height = end.Y - start.Y;
        var width = end.X - start.X;

        var image = GraphicsHelper.GetBitmapFromUrl(imageUrl, width, height).Result;

        canvas = DrawBitmapOnCanvas(canvas, image, start, end);

        return canvas;
    }

    private RGBLedCanvas DrawTextOnCanvas(RGBLedCanvas canvas, string text, RenderPoint start, RenderPoint? end)
    {
        var font = new RGBLedFont("fonts/6x10.bdf");

        var textLength = canvas.DrawText(font, start.X, start.Y, new Color(0, 255, 0), text);

        // TODO make text as big as fits in the section
        canvas.DrawText(font, start.X, start.Y, new Color(0, 255, 0), text);

        return canvas;
    }

    private RGBLedCanvas DrawBitmapOnCanvas(RGBLedCanvas canvas, SKBitmap bitmap, RenderPoint? start = null, RenderPoint? end = null)
    {
        // Loop through the pixels of the image and set the corresponding pixel in the RGBLedMatrix canvas
        var width = end?.X - start?.X ?? canvas.Width;
        var height = end?.Y - start?.Y ?? canvas.Height;

        var xOffset = (width - bitmap.Width) / 2;
        var yOffset = (height - bitmap.Height) / 2;

        var startX = start?.X ?? 0;
        var startY = start?.Y ?? 0;

        // Loop through the pixels of the image and set the corresponding pixel in the RGBLedMatrix canvas
        for (var y = 0; y < bitmap.Height; ++y)
        {
            for (var x = 0; x < bitmap.Width; ++x)
            {
                var pixel = bitmap.GetPixel(x, y);

                canvas.SetPixel(x + xOffset + startX, y + yOffset + startY, new Color(pixel.Red, pixel.Green, pixel.Blue));
            }
        }

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
            Brightness = 100
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