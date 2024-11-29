using System;

namespace TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Models;

internal class Gp5MixTable
{
    public sbyte Instrument { get; set; }
    public Gp5RseInstrument RseInstrument { get; set; }
    public sbyte Volume { get; set; }
    public sbyte Balance { get; set; }
    public sbyte Chorus { get; set; }
    public sbyte Reverb { get; set; }
    public sbyte Phaser { get; set; }
    public sbyte Tremolo { get; set; }
    public Gp5Tempo Tempo { get; set; }

    public byte? VolumeTransition { get; set; }
    public byte? BalanceTransition { get; set; }
    public byte? ChorusTransition { get; set; }
    public byte? ReverbTransition { get; set; }
    public byte? PhaserTransition { get; set; }
    public byte? TremoloTransition { get; set; }
    public byte? TempoTransition { get; set; }

    public Primary PrimaryFlags { get; set; }
    public sbyte WahWah { get; set; }
    public Gp5RseInstrumentEffect RseInstrumentEffect { get; set; }


    [Flags]
    public enum Primary : byte
    {
        // TODO: flags
    }
}
