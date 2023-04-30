public class RenderRequest 
{
    public int Width { get; set; }
    public int Height { get; set; }
    public RenderSection[]? Sections { get; set; }
}

public class RenderSection 
{
    public RenderPoint? Start { get; set; }
    public RenderPoint? End { get; set; }
    public Graphic? Graphic { get; set; }
}

public class Graphic 
{
    public GraphicType Type { get; set; }
    public string? Content { get; set; }
}

public class RenderPoint 
{
    public int X { get; set; }
    public int Y { get; set; }

    public RenderPoint(int x, int y) 
    {
        X = x;
        Y = y;
    }
}

public enum GraphicType 
{
    Text,
    Image,
    Gif
}