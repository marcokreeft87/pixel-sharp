using rpi_rgb_led_matrix_sharp;

namespace PixelSharp.Extensions;

public static class ClockExtension
{
    public static RGBLedCanvas DrawHands(this IPixelSharpMatrix matrix, RGBLedCanvas canvas)
    {
        // Draw the hour hand
        DrawHourHand(matrix, canvas);

        // Draw the minute hand
        DrawMinuteHand(matrix, canvas);

        return matrix.SwapCanvas(canvas);
    }

    public static RGBLedCanvas DrawClock(this IPixelSharpMatrix matrix)
    {
        // Draw the clock face
        var canvas = matrix.DrawCircle(0, 0, matrix.Width / 2, new Color(255, 255, 255));

        // Draw the hour markers
        canvas = DrawHourMarkers(matrix, canvas);

        return canvas;
    }

    private static RGBLedCanvas DrawHourMarkers(IPixelSharpMatrix matrix, RGBLedCanvas canvas)
    {
        for (var i = 0; i < 12; i++)
        {
            var angle = i * 30;
            var x = (int)(Math.Cos(angle * (Math.PI / 180)) * (matrix.Width / 2) * 0.9);
            var y = (int)(Math.Sin(angle * (Math.PI / 180)) * (matrix.Height / 2) * 0.9);
            canvas.SetPixel(x + (matrix.Width / 2), y + (matrix.Height / 2), new Color(255, 255, 255));
        }

        return canvas;
    }

    private static RGBLedCanvas DrawHourHand(IPixelSharpMatrix matrix, RGBLedCanvas canvas)
    {
        var hour = DateTime.Now.Hour;
        var hourAngle = (hour * 30) + (DateTime.Now.Minute / 2);
        var hourX = (int)(Math.Cos(hourAngle * (Math.PI / 180)) * (matrix.Width / 2) * 0.75);
        var hourY = (int)(Math.Sin(hourAngle * (Math.PI / 180)) * (matrix.Height / 2) * 0.75);

        canvas.DrawLine(matrix.Width / 2, matrix.Height / 2, hourX, hourY, new Color(255, 255, 255));

        return canvas;
    }

    private static RGBLedCanvas DrawMinuteHand(IPixelSharpMatrix matrix, RGBLedCanvas canvas)
    {
        var minute = DateTime.Now.Minute;
        var minuteAngle = minute * 6;
        var minuteX = (int)(Math.Cos(minuteAngle * (Math.PI / 180)) * (matrix.Width / 2) * 0.9);
        var minuteY = (int)(Math.Sin(minuteAngle * (Math.PI / 180)) * (matrix.Height / 2) * 0.9);

        canvas.DrawLine(matrix.Width / 2, matrix.Height / 2, minuteX, minuteY, new Color(255, 255, 255));

        return canvas;
    }
}
