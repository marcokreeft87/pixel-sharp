namespace PixelSharp.Models;

public class ClockSettings 
{
    public SettingsColor HourHandColor { get; set; } = new SettingsColor { R = 255, G = 0, B = 0 };

    public SettingsColor MinuteHandColor { get; set; } = new SettingsColor { R = 0, G = 255, B = 0 };

    public SettingsColor SecondHandColor { get; set; } = new SettingsColor { R = 0, G = 0, B = 255 };

    public SettingsColor ClockColor { get; set; } = new SettingsColor { R = 255, G = 255, B = 255 };

    public bool ShowSecondHand { get; set; } = true;
}