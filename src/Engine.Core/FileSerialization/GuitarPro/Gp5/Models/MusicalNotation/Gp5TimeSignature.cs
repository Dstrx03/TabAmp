namespace TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Models.MusicalNotation;

internal class Gp5TimeSignature
{
    public const int BeamGroupsLength = 4;

    public byte Numerator { get; set; }
    public byte? Denominator { get; set; }
    public byte[] BeamGroups { get; set; }
}
