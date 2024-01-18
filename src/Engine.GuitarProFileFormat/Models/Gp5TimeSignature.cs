namespace TabAmp.Engine.GuitarProFileFormat.Models;

public class Gp5TimeSignature
{
    public byte? Numerator { get; set; }
    public byte? Denominator { get; set; }
    public byte[] BeamsTodo { get; set; }// TODO: name, design
}
