namespace TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Models.MusicalNotation;

internal class Gp5TimeSignature
{
    public byte Numerator { get; set; }
    public byte? Denominator { get; set; }
    public Gp5TimeSignatureBeamGroups BeamGroups { get; set; }
}
