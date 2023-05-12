public class PixelDisplaySettings
{
    public int LedRows { get; set; }
    public int LedColumns { get; set;}
    public string? HardwareMapping { get; set; }
    public ClockSettings ClockSettings { get; set; } = new ClockSettings();
}