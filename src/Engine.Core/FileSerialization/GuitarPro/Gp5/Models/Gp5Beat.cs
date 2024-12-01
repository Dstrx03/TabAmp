﻿using System;

namespace TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Models;

internal class Gp5Beat
{
    public Primary PrimaryFlags { get; set; }
    public byte? Status { get; set; }
    public sbyte Duration { get; set; }
    public int? Tuplet { get; set; }
    public Gp5Chord? Chord { get; set; }
    public string? Text { get; set; }
    public Gp5BeatEffects? Effects { get; set; }
    public Gp5MixTable? MixTable { get; set; }
    public NotesPresence NotesPresenceFlags { get; set; }
    public Gp5Note[]? Notes { get; set; }
    public Secondary SecondaryFlags { get; set; }
    public byte? TODO { get; set; }


    [Flags]
    public enum Primary : byte
    {
        HasDot = 0x01,
        HasChord = 0x02,
        HasText = 0x04,
        HasEffects = 0x08,
        HasMixTable = 0x10,
        HasTuplet = 0x20,
        HasStatus = 0x40
    }

    [Flags]
    public enum NotesPresence : byte
    {
        HasSeventhStringNote = 0x01,
        HasSixthStringNote = 0x02,
        HasFifthStringNote = 0x04,
        HasFourthStringNote = 0x08,
        HasThirdStringNote = 0x10,
        HasSecondStringNote = 0x20,
        HasFirstStringNote = 0x40
    }

    [Flags]
    public enum Secondary : short
    {
        // TODO: naming
        TODO = 0x0800
    }
}
