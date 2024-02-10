using System;

namespace TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Models;

internal class Gp5PageSetup
{
    public int Width { get; set; }
    public int Height { get; set; }

    public int MarginLeft { get; set; }
    public int MarginRight { get; set; }
    public int MarginTop { get; set; }
    public int MarginBottom { get; set; }

    public int ScoreSizeProportion { get; set; }

    public HeaderAndFooter HeaderAndFooterFlags { get; set; }
    public string Title { get; set; }
    public string Subtitle { get; set; }
    public string Artist { get; set; }
    public string Album { get; set; }
    public string Words { get; set; }
    public string Music { get; set; }
    public string WordsAndMusic { get; set; }
    public string CopyrightFirstLine { get; set; }
    public string CopyrightSecondLine { get; set; }
    public string PageNumber { get; set; }


    [Flags]
    public enum HeaderAndFooter : short
    {
        DisplayTitle = 0x001,
        DisplaySubtitle = 0x002,
        DisplayArtist = 0x004,
        DisplayAlbum = 0x008,
        DisplayWords = 0x010,
        DisplayMusic = 0x020,
        DisplayWordsAndMusic = 0x040,
        DisplayCopyright = 0x080,
        DisplayPageNumber = 0x100
    }
}
