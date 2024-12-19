namespace TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Models.Effects;

internal class Gp5Bend
{
    public byte Type { get; set; }
    public int PitchShift { get; set; }
    public (int timePosition, int pitchShift, byte vibrato)[] Points { get; set; }
}
