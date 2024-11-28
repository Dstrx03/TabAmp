namespace TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Models;

internal class Gp5Bend
{
    public byte Shape { get; set; }
    public int Shift { get; set; }
    public (int time, int shift, byte vibrato)[] Points { get; set; }
}
