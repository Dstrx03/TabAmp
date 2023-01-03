namespace TabAmp.Models;

public class BendEffect
{
    public sbyte Type { get; set; }
    public int Value { get; set; }
    public int PointCount { get; set; }
    public List<(int position, int value, bool vibrato)> Points { get; set; }
}
