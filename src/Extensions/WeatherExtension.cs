using rpi_rgb_led_matrix_sharp;
using SkiaSharp;

namespace PixelSharp.Extensions;

public static class WeatherExtension
{
    // Example: https://m.media-amazon.com/images/I/61jw4wbabZL._AC_UF894,1000_QL80_.jpg
    public static void DrawWeather(this IPixelSharpMatrix matrix, RGBLedCanvas canvas)
    {
        var image = new SKBitmap(matrix.Width, matrix.Height);
        
        using (SKCanvas imageCanvas = new SKCanvas(image))
        {
            imageCanvas.Clear(SKColors.Black);

            // Draw the date top left
            var dateText = DateTime.Now.ToString("dd-MM");
            var dateTextPaint = new SKPaint() { Color = SKColors.Gray, TextSize = 10 };
            
            imageCanvas.DrawText(dateText, 0, 10, dateTextPaint);

            // Draw the time top center
            var timeText = DateTime.Now.ToString("HH:mm");
            var timeTextPaint = new SKPaint() { Color = SKColors.Gray, TextSize = 10 };
            var timeTextWidth = timeTextPaint.MeasureText(timeText);
            imageCanvas.DrawText(timeText, (matrix.Width / 2) - (timeTextWidth / 2), 10, timeTextPaint);

            // Draw the day in format ddd top right
            var dayText = DateTime.Now.ToString("ddd");
            var dayTextPaint = new SKPaint() { Color = SKColors.Gray, TextSize = 10 };
            var dayTextWidth = dayTextPaint.MeasureText(dayText);
            imageCanvas.DrawText(dayText, matrix.Width - dayTextWidth, 10, dayTextPaint);

            // Draw a gray line below the date/time/day
            var linePaint = new SKPaint() { Color = SKColors.Gray, StrokeWidth = 1 };
            imageCanvas.DrawLine(0, 15, matrix.Width, 15, linePaint);

            // Draw the names of the next 4 days below the line starting today in 2 letter format using DateTime with for loop evenly spaced
        }


        matrix.DrawBitmapOnCanvas(canvas, image);

        matrix.SwapCanvas(canvas);
    }

    private static string GetWeather()
    {
        var client = new HttpClient();
        var response = client.GetAsync("https://wttr.in/?format=3").Result;
        return response.Content.ReadAsStringAsync().Result;
    }
}
