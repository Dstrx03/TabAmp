namespace TabAmp.Models;

public class GP5Song
{
    public string Version { get; set; }
    public ScoreInformation ScoreInformation { get; set; }
    public Lyrics Lyrics { get; set; }
    public RSEMasterEffect RSEMasterEffect { get; set; }
    public PageSetup PageSetup { get; set; }
    public Tempo Tempo { get; set; }
    public sbyte Key { get; set; }
    public int Octave { get; set; }
    public List<MidiChannel> MidiChannels { get; set; }
    public List<(string, short)> MusicalDirections { get; set; }
    public int MeasureCount { get; set; }
    public int TrackCount { get; set; }
    public List<MeasureHeader> MeasureHeaders { get; set; }
    public List<Track> Tracks { get; set; }
    public List<Measure> Measures { get; set; }
}
