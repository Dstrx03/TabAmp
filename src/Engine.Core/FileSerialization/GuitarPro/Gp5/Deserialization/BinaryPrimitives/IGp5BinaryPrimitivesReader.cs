﻿using System.Threading.Tasks;
using TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Models.BinaryPrimitives;

namespace TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Deserialization.BinaryPrimitives;

internal interface IGp5BinaryPrimitivesReader
{
    ValueTask<byte> ReadByteAsync();
    ValueTask<sbyte> ReadSignedByteAsync();
    ValueTask<short> ReadShortAsync();
    ValueTask<int> ReadIntAsync();
    ValueTask<float> ReadFloatAsync();
    ValueTask<double> ReadDoubleAsync();
    ValueTask<Gp5Bool> ReadBoolAsync();
    ValueTask<Gp5Color> ReadColorAsync();
}
