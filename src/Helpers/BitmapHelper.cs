using SkiaSharp;

public static class GraphicsHelper 
{
    public static SKBitmap GetBitmapFromPath(string path, int screenWidth, int screenHeight)
    {
        var bitmap = SKBitmap.Decode(path);

        return ResizeBitmap(screenWidth, screenHeight, bitmap);
    }

    public static async Task<SKBitmap> GetBitmapFromUrl(string url, int screenWidth, int screenHeight)
    {
        using var client = new HttpClient();
        using var response = await client.GetAsync(url);
        using var stream = await response.Content.ReadAsStreamAsync();

        var bitmap = SKBitmap.Decode(stream);

        return ResizeBitmap(screenWidth, screenHeight, bitmap);
    }  

    public static (List<SKBitmap> frames, List<int> durations) GetGifFromUrl(string url, int screenWidth, int screenHeight)
    {
        using var client = new HttpClient();
        using var response = client.GetAsync(url).Result.Content.ReadAsStream();
        using (var codec = SKCodec.Create(response))
        {
            var frameCount = codec.FrameCount;
            Console.WriteLine($"Frame count: {codec.FrameCount}");

            var info = codec.Info;
            var count = codec.FrameCount;

            // Get all frames from gif and duration and add to list
            var bitmap = new SKBitmap(info);
            var frames = new List<SKBitmap>();
            var frameLengths = new List<int>();

            for (int i = 0; i < count; i++)
            {   
                var opts = new SKCodecOptions(i);

                if (codec?.GetPixels(info, bitmap.GetPixels(), opts) == SKCodecResult.Success)
                {
                    bitmap.NotifyPixelsChanged();

                    var resized = ResizeBitmap(screenWidth, screenHeight, bitmap);

                    frames.Add(resized);
                    frameLengths.Add(codec.FrameInfo[i].Duration);
                }
            }
            
            return (frames, frameLengths);
        }
    }  

    private static SKBitmap ResizeBitmap(int screenWidth, int screenHeight, SKBitmap bitmap)
    {
        int targetWidth, targetHeight;
        CalculateDimensions(bitmap, screenWidth, screenHeight, out targetWidth, out targetHeight);

        // Create a new bitmap with the desired dimensions
        var resizedBitmap = new SKBitmap(targetWidth, targetHeight);

        // Create a new surface from the resized bitmap
        using (var surface = new SKCanvas(resizedBitmap))
        {
            // Draw the original bitmap onto the new surface, scaling it to fit while maintaining aspect ratio
            surface.DrawBitmap(bitmap, SKRect.Create(targetWidth, targetHeight));
        }

        return resizedBitmap;
    }

    private static void CalculateDimensions(SKBitmap bitmap, int screenWidth, int screenHeight, out int targetWidth, out int targetHeight)
    {
        // Get the size of the image
        int imageWidth = bitmap.Width;
        int imageHeight = bitmap.Height;

        // Check if the image needs to be resized
        if (imageWidth > screenWidth || imageHeight > screenHeight)
        {
            // Calculate the new size while maintaining aspect ratio
            if (imageWidth > imageHeight)
            {
                targetWidth = screenWidth;
                targetHeight = (int)(imageHeight / (float)imageWidth * targetWidth);
            }
            else
            {
                targetHeight = screenHeight;
                targetWidth = (int)(imageWidth / (float)imageHeight * targetHeight);
            }
        }
        else 
        {
            targetWidth = imageWidth;
            targetHeight = imageHeight;
        }
    }
}