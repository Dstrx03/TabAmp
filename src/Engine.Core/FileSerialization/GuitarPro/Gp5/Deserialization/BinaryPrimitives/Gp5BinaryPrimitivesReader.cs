using System;
using System.Threading.Tasks;
using TabAmp.Engine.Core.FileSerialization.Common.Components.IO.Serial;
using TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Models.BinaryPrimitives;
using SystemBinaryPrimitives = System.Buffers.Binary.BinaryPrimitives;

namespace TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Deserialization.BinaryPrimitives;

internal class Gp5BinaryPrimitivesReader : IGp5BinaryPrimitivesReader
{
    private readonly ISerialFileReader _fileReader;

    private const int ByteSize = 1;
    private const int ShortSize = 2;
    private const int IntSize = 4;
    private const int FloatSize = 4;
    private const int DoubleSize = 8;
    private const int ColorSize = 4;

    private const int ColorRedPosition = 0, ColorGreenPosition = 1, ColorBluePosition = 2, Color_A01Position = 3;

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
    private static ISerialFileReader.Convert<short> ConvertToShort { get; } = SystemBinaryPrimitives.ReadInt16LittleEndian;
    private static ISerialFileReader.Convert<int> ConvertToInt { get; } = SystemBinaryPrimitives.ReadInt32LittleEndian;
    private static ISerialFileReader.Convert<float> ConvertToFloat { get; } = SystemBinaryPrimitives.ReadSingleLittleEndian;
    private static ISerialFileReader.Convert<double> ConvertToDouble { get; } = SystemBinaryPrimitives.ReadDoubleLittleEndian;
    private static ISerialFileReader.Convert<Gp5Bool> ConvertToBool { get; } = ReadBool;
    private static ISerialFileReader.Convert<Gp5Color> ConvertToColor { get; } = ReadColor;

    private static byte ReadByte(ReadOnlySpan<byte> source) => source[0];

    private static sbyte ReadSignedByte(ReadOnlySpan<byte> source) => (sbyte)ReadByte(source);

    private static Gp5Bool ReadBool(ReadOnlySpan<byte> source) => (Gp5Bool)ReadByte(source);

    private static Gp5Color ReadColor(ReadOnlySpan<byte> source) => new()
    {
        Red = source[ColorRedPosition],
        Green = source[ColorGreenPosition],
        Blue = source[ColorBluePosition],
        _A01 = source[Color_A01Position]
    };
}
