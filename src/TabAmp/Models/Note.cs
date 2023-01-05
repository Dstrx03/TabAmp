namespace TabAmp.Models;

public class Note
{
    public int StringNumber { get; set; }
    public byte Flags { get; set; }
    public byte Type { get; set; }
    public sbyte Velocity { get; set; }
    public sbyte Value { get; set; }
    public sbyte LeftHandFinger { get; set; }
    public sbyte RightHandFinger { get; set; }
    public double DurationPercent { get; set; }
    public byte Flags2 { get; set; }
    public NoteEffect Effect { get; set; }
}
