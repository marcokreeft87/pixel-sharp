using rpi_rgb_led_matrix_sharp;

public interface IPixelSharpMatrix 
{
    RGBLedCanvas Render(RGBLedCanvas canvas);
    void ShowLogo();
    void ScrollText(string text, CancellationToken cancellationToken);
    void ShowBitmapFromUrl(string imageUrl);
}