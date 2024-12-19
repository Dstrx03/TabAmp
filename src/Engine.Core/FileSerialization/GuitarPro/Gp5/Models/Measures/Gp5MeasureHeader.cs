using System;
using TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Models.DocumentComponents;
using TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Models.MusicalNotation;

namespace TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Models.Measures;

internal class Gp5MeasureHeader
{
    public Primary PrimaryFlags { get; set; }
    public Gp5TimeSignature? TimeSignature { get; set; }
    public byte? RepeatsCount { get; set; }
    public Gp5Marker? Marker { get; set; }
    public Gp5KeySignature? KeySignature { get; set; }
    public AlternateEndings AlternateEndingsFlags { get; set; }
    public byte TripletFeel { get; set; }


    [Flags]
    public enum Primary : byte
    {
        HasTimeSignature = 0x01,
        HasTimeSignatureDenominator = 0x02,
        HasRepeatOpen = 0x04,
        HasRepeatClose = 0x08,
        HasAlternateEndings = 0x10,
        HasMarker = 0x20,
        HasKeySignature = 0x40,
        HasDoubleBar = 0x80
    }

    [Flags]
    public enum AlternateEndings : byte
    {
        Ending1 = 0x01,
        Ending2 = 0x02,
        Ending3 = 0x04,
        Ending4 = 0x08,
        Ending5 = 0x10,
        Ending6 = 0x20,
        Ending7 = 0x40,
        Ending8 = 0x80
    }

    #region Unknown & Anonymous data
    public byte _A01 { get; set; }
    #endregion
}
