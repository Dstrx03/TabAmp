namespace TabAmp.Models;

public class Chord
{
    public bool NewFormat { get; set; }
    public string Name { get; set; }
    public int FirstFret { get; set; }
    public List<int> Strings { get; set; }
    public bool Sharp { get; set; }
    public byte Unknown0 { get; set; }
    public byte Unknown1 { get; set; }
    public byte Unknown2 { get; set; }
    public byte Root { get; set; }
    public byte Type { get; set; }
    public byte Extension { get; set; }
    public int Bass { get; set; }
    public int Tonality { get; set; }
    public bool Add { get; set; }
    public byte Fifth { get; set; }
    public byte Ninth { get; set; }
    public byte Eleventh { get; set; }
    public byte BarresCount { get; set; }
    public List<byte> BarreFrets { get; set; }
    public List<byte> BarreStarts { get; set; }
    public List<byte> BarreEnds { get; set; }
    public List<bool> Omissions { get; set; }
    public byte Unknown3 { get; set; }
    public List<sbyte> Fingerings { get; set; }
    public bool Show { get; set; }
}
