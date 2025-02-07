namespace TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Models.Effects;

internal class Gp5Harmonic
{
    public byte Type { get; set; }
    public byte? Note { get; set; }
    public sbyte? Accidental { get; set; }
    public byte? Octave { get; set; }
    public byte? Fret { get; set; }
}
