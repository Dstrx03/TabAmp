namespace TabAmp.Models;

public class Lyrics
{
    public int TrackChoice { get; set; }
    public List<(int startingMeasure, string line)> Lines { get; set; }
}
