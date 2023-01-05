namespace TabAmp.Models;

public class NoteEffect
{
    public sbyte Flags1 { get; set; }
    public sbyte Flags2 { get; set; }
    public BendEffect Bend { get; set; }
    public GraceEffect Grace { get; set; }
    public TremoloPickingEffect TremoloPicking { get; set; }
    public SlidesEffect Slides { get; set; }
    public HarmonicEffect Harmonic { get; set; }
    public TrillEffect Trill { get; set; }
}
