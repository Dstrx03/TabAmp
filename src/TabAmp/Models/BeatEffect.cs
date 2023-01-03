namespace TabAmp.Models;

public class BeatEffect
{
    public sbyte Flags1 { get; set; }
    public sbyte Flags2 { get; set; }
    public sbyte SlapEffect { get; set; }
    public BendEffect TremoloBar { get; set; }
    public BeatStroke Stroke { get; set; }
    public sbyte PickStroke { get; set; }
}
