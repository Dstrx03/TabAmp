namespace TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Models.MusicalNotation;

internal class Gp5Chord
{
    public bool Sharp { get; set; }
    public byte Root { get; set; }
    public byte Type { get; set; }
    public byte Extension { get; set; }
    public int Bass { get; set; }
    public int Tonality { get; set; }
    public bool Add { get; set; }
    public string Name { get; set; }
    public byte FifthTonality { get; set; }
    public byte NinthTonality { get; set; }
    public byte EleventhTonality { get; set; }
    public int Fret { get; set; }
    public int[] Frets { get; set; }
    public byte BarresCount { get; set; }
    public byte[] BarreFrets { get; set; }
    public byte[] BarreStarts { get; set; }
    public byte[] BarreEnds { get; set; }
    public bool[] Omissions { get; set; }
    public sbyte[] Fingerings { get; set; }
    public bool Show { get; set; }


    #region Unknown & Anonymous data
    public byte _A01 { get; set; }
    public byte _A02 { get; set; }
    public byte _A03 { get; set; }
    public byte _B01 { get; set; }
    #endregion
}
