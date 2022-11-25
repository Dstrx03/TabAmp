namespace TabAmp.Models;

public class PageSetup
{
    public int Width { get; set; }
    public int Height { get; set; }
    public int MarginLeft { get; set; }
    public int MarginRight { get; set; }
    public int MarginTop { get; set; }
    public int MarginBottom { get; set; }
    public int ScoreSizeProportion { get; set; }
    public short HeaderAndFooter { get; set; }
    public string Title { get; set; }
    public string Subtitle { get; set; }
    public string Artist { get; set; }
    public string Album { get; set; }
    public string Words { get; set; }
    public string Music { get; set; }
    public string WordsAndMusic { get; set; }
    public string Copyright { get; set; }
    public string CopyrightAdditional { get; set; }
    public string PageNumber { get; set; }
}
