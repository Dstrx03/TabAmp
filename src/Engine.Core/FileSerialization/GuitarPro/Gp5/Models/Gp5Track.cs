﻿using System;

namespace TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Models;

internal class Gp5Track
{
    public const int NameMaxLength = 40;
    public const int StringsTuningLength = 7;
    public const int RseEqualizerBandsCount = 3;

    public Primary PrimaryFlags { get; set; }
    public string Name { get; set; }
    public int StringsCount { get; set; }
    public int[] StringsTuning { get; } = new int[StringsTuningLength];
    public int Port { get; set; }
    public int MainChannel { get; set; }
    public int EffectChannel { get; set; }
    public int FretsCount { get; set; }
    public int CapoFret { get; set; }
    public int Color { get; set; }
    public Secondary SecondaryFlags { get; set; }
    public byte RseAutoAccentuation { get; set; }
    public byte MidiBank { get; set; }
    public byte RseHumanPlaying { get; set; }
    public int Instrument { get; set; }
    public int RseSoundBank { get; set; }
    public Gp5RseEqualizer RseEqualizer { get; set; }
    public string RseEffectName { get; set; }
    public string RseEffectCategoryName { get; set; }


    [Flags]
    public enum Primary : byte
    {
        PercussionTypeTrack = 0x01,// TODO: consistent naming
        Simulate12StringedGuitar = 0x02,
        ParticularForBanjo5thString = 0x04,
        _A01 = 0x08,
        Solo = 0x10,
        Mute = 0x20,
        UseRse = 0x40,
        IndicateTuningOnTheScore = 0x80
    }

    [Flags]
    public enum Secondary : short
    {
        // TODO: names that make sense
        Tablature_TODO = 0x0001,
        Notation_TODO = 0x0002,
        DiagramsAreBelow_TODO = 0x0004,
        ShowRhythm_TODO = 0x0008,
        ForceHorizontal_TODO = 0x0010,
        ForceChannels_TODO = 0x0020,
        DiagramList_TODO = 0x0040,
        DiagramsInScore_TODO = 0x0080,
        Unknown0_TODO = 0x0100,
        AutoLetRing_TODO = 0x0200,
        AutoBrush_TODO = 0x0400,
        ExtendRhythmic_TODO = 0x0800
    }

    #region Unknown & Anonymous data
    public int _A01 { get; set; }
    public int _A02 { get; set; }
    public int _A03 { get; set; }
    public byte _B01 { get; set; }
    public byte _B02 { get; set; }
    public byte _B03 { get; set; }
    public byte _B04 { get; set; }
    public byte _B05 { get; set; }
    public byte _B06 { get; set; }
    public byte _B07 { get; set; }
    public byte _B08 { get; set; }
    public byte _B09 { get; set; }
    public byte _B10 { get; set; }
    public short _C01 { get; set; }
    public int _D01 { get; set; }
    public int _E01 { get; set; }
    #endregion
}
