using rpi_rgb_led_matrix_sharp;

public interface IPixelSharpMatrix 
{
    int Width { get; }
    int Height { get; }
    RGBLedCanvas SwapCanvas(RGBLedCanvas canvas);
    void DrawLogo();
    void DrawText(string text, CancellationToken cancellationToken);
    void ScrollText(string text, CancellationToken cancellationToken);
    void DrawBitmapFromUrl(string imageUrl);
    void DrawBitmapFromBase64(string base64string);
    void DrawGifFromUrl(string imageUrl, CancellationToken cancellationToken);
    void DrawGifFromBase64(string base64string, CancellationToken cancellationToken);
    void Render(RenderRequest request, CancellationToken cancellationToken);
}