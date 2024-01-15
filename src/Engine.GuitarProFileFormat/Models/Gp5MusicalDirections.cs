namespace TabAmp.Engine.GuitarProFileFormat.Models;

public class Gp5MusicalDirections
{
    public short Coda { get; set; }
    public short DoubleCoda { get; set; }
    public short Segno { get; set; }
    public short SegnoSegno { get; set; }
    public short Fine { get; set; }

    public short DaCapo { get; set; }
    public short DaCapoAlCoda { get; set; }
    public short DaCapoAlDoubleCoda { get; set; }
    public short DaCapoAlFine { get; set; }
    public short DaSegno { get; set; }
    public short DaSegnoAlCoda { get; set; }
    public short DaSegnoAlDoubleCoda { get; set; }
    public short DaSegnoAlFine { get; set; }
    public short DaSegnoSegno { get; set; }
    public short DaSegnoSegnoAlCoda { get; set; }
    public short DaSegnoSegnoAlDoubleCoda { get; set; }
    public short DaSegnoSegnoAlFine { get; set; }
    public short DaCoda { get; set; }
    public short DaDoubleCoda { get; set; }
}
