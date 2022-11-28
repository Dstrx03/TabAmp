namespace TabAmp.Models;

public class MeasureHeader
{
    public byte Flags { get; set; }
    public bool IsRepeatOpen { get; set; }
    public bool HasDoubleBar { get; set; }
    public sbyte? Numerator { get; set; }
    public sbyte? Denominator { get; set; }
    public sbyte? RepeatClose { get; set; }
    public Marker Marker { get; set; }
    public sbyte? Root { get; set; }
    public sbyte? Type { get; set; }
    public byte? RepeatAlternative { get; set; }
    public List<byte> Beams { get; set; }
    public byte TripletFeel { get; set; }
}
