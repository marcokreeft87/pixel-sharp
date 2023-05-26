namespace PixelSharp.Models;

public class PixelDisplaySettings
{
    public string? IpAddress { get; set; }
    public int LedRows { get; set; }
    public int LedColumns { get; set; }
    public string? HardwareMapping { get; set; }
    public ClockSettings ClockSettings { get; set; } = new ClockSettings();
}
