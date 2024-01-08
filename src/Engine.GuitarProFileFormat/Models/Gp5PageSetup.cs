using System;

namespace TabAmp.Engine.GuitarProFileFormat.Models;

public class Gp5PageSetup
{
    public int Width { get; set; }
    public int Height { get; set; }
    public int MarginLeft { get; set; }
    public int MarginRight { get; set; }
    public int MarginTop { get; set; }
    public int MarginBottom { get; set; }
    public int ScoreSizeProportion { get; set; }
    public HeaderAndFooterFlags HeaderAndFooter { get; set; }
    public string Title { get; set; }
    public string Subtitle { get; set; }
    public string Artist { get; set; }
    public string Album { get; set; }
    public string Words { get; set; }
    public string Music { get; set; }
    public string WordsAndMusic { get; set; }
    public string Copyright { get; set; }

    [Flags]
    public enum HeaderAndFooterFlags : short
    {
        Title = 0x001,
        Subtitle = 0x002,
        Artist = 0x004,
        Album = 0x008,
        Words = 0x010,
        Music = 0x020,
        WordsAndMusic = 0x040,
        Copyright = 0x080,
        PageNumber = 0x100
    }
}
