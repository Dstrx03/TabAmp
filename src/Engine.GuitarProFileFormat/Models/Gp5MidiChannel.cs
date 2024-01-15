namespace TabAmp.Engine.GuitarProFileFormat.Models;

public class Gp5MidiChannel
{
    public int Instrument { get; set; }
    public sbyte Volume { get; set; }
    public sbyte Balance { get; set; }
    public sbyte Chorus { get; set; }
    public sbyte Reverb { get; set; }
    public sbyte Phaser { get; set; }
    public sbyte Tremolo { get; set; }

    public sbyte Blank1 { get; set; }
    public sbyte Blank2 { get; set; }
}
