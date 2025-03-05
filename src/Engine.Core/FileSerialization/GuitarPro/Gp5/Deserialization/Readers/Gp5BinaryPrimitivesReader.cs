﻿using System;
using System.Buffers.Binary;
using System.Threading.Tasks;
using TabAmp.Engine.Core.FileSerialization.Common.Components.IO.Serial;
using TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Models.BinaryPrimitives;

namespace TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Deserialization.Readers;

internal class Gp5BinaryPrimitivesReader : IGp5BinaryPrimitivesReader
{
    private readonly ISerialFileReader _fileReader;

    private const int ByteSize = 1;
    private const int ShortSize = 2;
    private const int IntSize = 4;
    private const int FloatSize = 4;
    private const int DoubleSize = 8;
    private const int ColorSize = 4;

    public Gp5BinaryPrimitivesReader(ISerialFileReader fileReader) =>
        _fileReader = fileReader;

    public ValueTask<byte> ReadByteAsync() =>
        _fileReader.ReadBytesAsync(ByteSize, ConvertToByte);

    public ValueTask<sbyte> ReadSignedByteAsync() =>
        _fileReader.ReadBytesAsync(ByteSize, ConvertToSignedByte);

    public ValueTask<short> ReadShortAsync() =>
        _fileReader.ReadBytesAsync(ShortSize, ConvertToShort);

    public ValueTask<int> ReadIntAsync() =>
        _fileReader.ReadBytesAsync(IntSize, ConvertToInt);

    public ValueTask<float> ReadFloatAsync() =>
        _fileReader.ReadBytesAsync(FloatSize, ConvertToFloat);

    public ValueTask<double> ReadDoubleAsync() =>
        _fileReader.ReadBytesAsync(DoubleSize, ConvertToDouble);

    public ValueTask<Gp5Bool> ReadBoolAsync() =>
        _fileReader.ReadBytesAsync(ByteSize, ConvertToBool);

    public ValueTask<Gp5Color> ReadColorAsync() =>
         _fileReader.ReadBytesAsync(ColorSize, ConvertToColor);

    private static ISerialFileReader.Convert<byte> ConvertToByte { get; } = ReadByte;
    private static ISerialFileReader.Convert<sbyte> ConvertToSignedByte { get; } = ReadSignedByte;
    private static ISerialFileReader.Convert<short> ConvertToShort { get; } = BinaryPrimitives.ReadInt16LittleEndian;
    private static ISerialFileReader.Convert<int> ConvertToInt { get; } = BinaryPrimitives.ReadInt32LittleEndian;
    private static ISerialFileReader.Convert<float> ConvertToFloat { get; } = BinaryPrimitives.ReadSingleLittleEndian;
    private static ISerialFileReader.Convert<double> ConvertToDouble { get; } = BinaryPrimitives.ReadDoubleLittleEndian;
    private static ISerialFileReader.Convert<Gp5Bool> ConvertToBool { get; } = ReadBool;
    private static ISerialFileReader.Convert<Gp5Color> ConvertToColor { get; } = Gp5Color.Read;

    private static byte ReadByte(ReadOnlySpan<byte> source) => source[0];
    private static sbyte ReadSignedByte(ReadOnlySpan<byte> source) => (sbyte)ReadByte(source);
    private static Gp5Bool ReadBool(ReadOnlySpan<byte> source) => (Gp5Bool)ReadByte(source);
}
