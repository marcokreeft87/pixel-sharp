using PixelSharp.Settings;
using rpi_rgb_led_matrix_sharp;
using SkiaSharp;

public interface IPixelSharpMatrix 
{
    int Width { get; }
    int Height { get; }
    PixelDisplaySettings DisplaySettings { get; }
    RGBLedCanvas SwapCanvas(RGBLedCanvas canvas);
    RGBLedCanvas CreateCanvas();
    void DrawLogo();
    void DrawText(string text, CancellationToken cancellationToken);
    void ScrollText(string text, CancellationToken cancellationToken);
    void DrawBitmapFromUrl(string imageUrl);
    void DrawBitmapFromBase64(string base64string);
    void DrawGifFromUrl(string imageUrl, CancellationToken cancellationToken);
    void DrawGifFromBase64(string base64string, CancellationToken cancellationToken);
    void Render(RenderRequest request, CancellationToken cancellationToken);
    RGBLedCanvas DrawBitmapOnCanvas(RGBLedCanvas canvas, SKBitmap bitmap, RenderPoint? start = null, RenderPoint? end = null);
}