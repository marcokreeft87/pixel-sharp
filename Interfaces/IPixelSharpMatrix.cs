using rpi_rgb_led_matrix_sharp;

public interface IPixelSharpMatrix 
{
    RGBLedCanvas Render(RGBLedCanvas canvas);
    void DrawLogo();
    void DrawText(string text, CancellationToken cancellationToken);
    void ScrollText(string text, CancellationToken cancellationToken);
    void DrawBitmapFromUrl(string imageUrl);
    void DrawGifFromUrl(string imageUrl);
}