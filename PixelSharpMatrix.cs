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

    public void DrawText(string text, CancellationToken cancellationToken)
    {
        text = text ?? "This is a static text.";

        var canvas = _matrix.CreateOffscreenCanvas();
        var font = new RGBLedFont("fonts/6x10.bdf");

        var textLength = canvas.DrawText(font, 1, 6, new Color(0, 255, 0), text);

        canvas = _matrix.CreateOffscreenCanvas();

        canvas.Clear();

        canvas.DrawText(font, 0, 20, new Color(0, 255, 0), text);

        canvas = Render(canvas);
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
                canvas = Render(canvas);

                Thread.Sleep(50);
            }

            animationCount++;
            x = canvas.Width;
        }
    }

    public void DrawLogo() => DrawBitmapFromPath("logo.png");

    public void DrawGifFromUrl(string imageUrl)
    {
        var codec = GraphicsHelper.GetGifFromUrl(imageUrl, _ledRows, _ledColumns).Result;

        // Load the GIF stream into an SKCodec
        if (codec.FrameCount == 0)
        {
            // Error handling: the GIF has no frames
            return;
        }
        
        var canvas = _matrix.CreateOffscreenCanvas();

        var info = codec.Info;
        var count = codec.FrameCount;

        var bitmap = new SKBitmap(info);
        var frames = new List<SKBitmap>();
        var frameLengths = new List<int>();

        for (int i = 0; i < count; i++)
        {   
            var opts = new SKCodecOptions(i);

            if (codec?.GetPixels(info, bitmap.GetPixels(), opts) == SKCodecResult.Success)
			{
				bitmap.NotifyPixelsChanged();
                
                frames.Add(bitmap);
                frameLengths.Add(codec.FrameInfo[i].Duration);
			}

        }

    // // Decode the GIF using SkiaSharp
    // var frames = new List<SKBitmap>();
    // ;

    // // Extract each frame from the GIF
    // for (int i = 0; i < codec.FrameCount; i++)
    // {
    //     // Get the next frame
    //     codec.GetFrameInfo(i, out var frameInfo);
    //     var bitmap = SKBitmap.Decode(codec.GetPixels(frameInfo, ));
    //     var bitmap = SKBitmap.Decode();
    //     frames.Add(bitmap);

    //     // Get the delay time for the frame
    //     //var frameInfo = codec.FrameInfo[i];
    //     var delayTime = frameInfo.Duration * 10; // Convert from 1/100th of a second to milliseconds
    //     frameLengths.Add(delayTime);
    // }

    // // Resize each frame to fit the canvas
    // var canvasWidth = canvas.Width;
    // var canvasHeight = canvas.Height;
    // foreach (var bitmap in frames)
    // {
    //     var resizedBitmap = bitmap.Resize(new SKImageInfo(canvasWidth, canvasHeight), SKFilterQuality.High);
    //     bitmap.Dispose();
    //     frames[frames.IndexOf(bitmap)] = resizedBitmap;
    // }

    // // Display the frames on the canvas
    // var currentFrameIndex = 0;
    // var currentFrameStartTime = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
    // var animationComplete = false;
    // while (!animationComplete)
    // {
    //     var currentFrame = frames[currentFrameIndex];
    //     canvas.Clear(SKColors.Black);
    //     canvas.DrawBitmap(currentFrame, 0, 0);

    //     // Wait for the specified amount of time before displaying the next frame
    //     var currentFrameLength = frameLengths[currentFrameIndex];
    //     var timeSinceFrameStart = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond - currentFrameStartTime;
    //     if (timeSinceFrameStart >= currentFrameLength)
    //     {
    //         currentFrameStartTime = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
    //         currentFrameIndex = (currentFrameIndex + 1) % frames.Count;

    //         if (currentFrameIndex == 0)
    //         {
    //             animationComplete = true;
    //         }
    //     }

    //     canvas = Render(canvas);
    // }

    // // Clean up the frames
    // foreach (var bitmap in frames)
        // {
        //     bitmap.Dispose();
        // }
    }

    public void DrawBitmapFromUrl(string imageUrl)
    {
        var bitmap = GraphicsHelper.GetBitmapFromUrl(imageUrl, _ledRows, _ledColumns).Result;

        var canvas = _matrix.CreateOffscreenCanvas();
        canvas = DrawBitmapOnCanvas(canvas, bitmap);

        Render(canvas);
    }

    public void DrawBitmapFromPath(string path)
    {
        var canvas = _matrix.CreateOffscreenCanvas();

        var bitmap = GraphicsHelper.GetBitmapFromPath(path, _ledRows, _ledColumns);

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