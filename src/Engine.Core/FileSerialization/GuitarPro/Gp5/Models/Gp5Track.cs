using System;

namespace TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Models;

internal class Gp5Track
{
    public const int NameStringMaxLength = 40;

    public Primary PrimaryFlags { get; set; }
    public string Name { get; set; }


    [Flags]
    public enum Primary : byte
    {
    }
}
