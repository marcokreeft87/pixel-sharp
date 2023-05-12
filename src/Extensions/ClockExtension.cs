using rpi_rgb_led_matrix_sharp;
using SkiaSharp;

namespace PixelSharp.Extensions;

public static class ClockExtension
{
    public static void DrawClock(this IPixelSharpMatrix matrix, RGBLedCanvas matrixCanvas)
    {
        // Create a new bitmap image using SkiaSharp
        var image = new SKBitmap(matrix.Width, matrix.Height);
        using (SKCanvas canvas = new SKCanvas(image))
        {
            // Clear the canvas with black color
            canvas.Clear(SKColors.Black);

            var centerX = matrix.Width / 2;
            var centerY = matrix.Height / 2;

            // Draw the clock circle
            var clockSettings = matrix.DisplaySettings.ClockSettings;
            var clockColor = new SKColor((byte)clockSettings.ClockColor.R, (byte)clockSettings.ClockColor.G, (byte)clockSettings.ClockColor.B);
            using (SKPaint clockCirclePaint = new SKPaint() { Color = clockColor, Style = SKPaintStyle.Stroke, StrokeWidth = 2 })
            {
                canvas.DrawCircle(centerX, centerY, (float)(matrix.Height / 2.2), clockCirclePaint);
            }

            // Draw the hour markers
            DrawHourMarkers(matrix, canvas);

            // Draw the clock hands
            DrawHands(matrix, canvas, centerX, centerY);

            // Draw seconds hand
            if(matrix.DisplaySettings.ClockSettings.ShowSecondHand)
                DrawSecondsHand(matrix, canvas, centerX, centerY);
        }

        matrix.DrawBitmapOnCanvas(matrixCanvas, image);

        matrix.SwapCanvas(matrixCanvas);
    }

    private static void DrawSecondsHand(IPixelSharpMatrix matrix, SKCanvas canvas, int centerX, int centerY)
    {
        var now = DateTime.Now;
        var secondsAngle = now.Second * Math.PI / 30;
        var secondsX = (int)(centerX + (double)matrix.Width / 2.2 * Math.Sin(secondsAngle));
        var secondsY = (int)(centerY + (double)matrix.Height / 2.2 * -Math.Cos(secondsAngle));

        var clockSettings = matrix.DisplaySettings.ClockSettings;
        var secopndColor = new SKColor((byte)clockSettings.SecondHandColor.R, (byte)clockSettings.SecondHandColor.G, (byte)clockSettings.SecondHandColor.B);
        using (SKPaint secondsHandPaint = new SKPaint() { Color = secopndColor, StrokeWidth = 2 })
        {
            canvas.DrawLine(centerX, centerY, secondsX, secondsY, secondsHandPaint);
        }
    }

    private static void DrawHourMarkers(IPixelSharpMatrix matrix, SKCanvas canvas)
    {
        var clockSettings = matrix.DisplaySettings.ClockSettings;
        var clockColor = new SKColor((byte)clockSettings.ClockColor.R, (byte)clockSettings.ClockColor.G, (byte)clockSettings.ClockColor.B);

        for (int i = 0; i < 12; i++)
        {
            var angle = i * 30;
            var x = (int)(Math.Cos(angle * (Math.PI / 180)) * (matrix.Width / 2) * 0.9);
            var y = (int)(Math.Sin(angle * (Math.PI / 180)) * (matrix.Height / 2) * 0.9);

            using (SKPaint hourMarkerPaint = new SKPaint() { Color = clockColor, StrokeWidth = 2 })
            {
                canvas.DrawPoint(x + (matrix.Width / 2), y + (matrix.Height / 2), hourMarkerPaint);
            }
        }
    }

    private static void DrawHands(IPixelSharpMatrix matrix, SKCanvas canvas, int centerX, int centerY)
    {
        var now = DateTime.Now;
        var hourAngle = (now.Hour % 12 + now.Minute / 60.0) * Math.PI / 6;
        var minuteAngle = now.Minute * Math.PI / 30;
        var hourX = (int)(centerX + (double)matrix.Width / 3 * Math.Sin(hourAngle));
        var hourY = (int)(centerY + (double)matrix.Height / 3 * -Math.Cos(hourAngle));
        var minuteX = (int)(centerX + (double)matrix.Width / 2.2 * Math.Sin(minuteAngle));
        var minuteY = (int)(centerY + (double)matrix.Height / 2.2 * -Math.Cos(minuteAngle));

        var clockSettings = matrix.DisplaySettings.ClockSettings;
        var hourColor = new SKColor((byte)clockSettings.HourHandColor.R, (byte)clockSettings.HourHandColor.G, (byte)clockSettings.HourHandColor.B);
        using (SKPaint hourHandPaint = new SKPaint() { Color = hourColor, StrokeWidth = 2 })
        {
            canvas.DrawLine(centerX, centerY, hourX, hourY, hourHandPaint);
        }
        
        var minuteColor = new SKColor((byte)clockSettings.MinuteHandColor.R, (byte)clockSettings.MinuteHandColor.G, (byte)clockSettings.MinuteHandColor.B);
        using (SKPaint minuteHandPaint = new SKPaint() { Color = minuteColor, StrokeWidth = 2 })
        {
            canvas.DrawLine(centerX, centerY, minuteX, minuteY, minuteHandPaint);
        }
    }
}
