using System.Buffers.Binary;
using System.Threading.Tasks;
using TabAmp.Engine.Core.FileSerialization.Common.Components.SerialFileReader;

namespace TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Deserialization;

internal class Gp5BinaryPrimitivesReader : IGp5BinaryPrimitivesReader
{
    private readonly ISerialFileReader _fileReader;

    private const int ByteSize = 1;
    private const int ShortSize = 2;
    private const int IntSize = 4;
    private const int FloatSize = 4;
    private const int DoubleSize = 8;

    public Gp5BinaryPrimitivesReader(ISerialFileReader fileReader) =>
        _fileReader = fileReader;

    public ValueTask<byte> ReadByteAsync() =>
        ReadByteValueAsync();

    public async ValueTask<sbyte> ReadSignedByteAsync() =>
        (sbyte)await ReadByteValueAsync();

    public async ValueTask<short> ReadShortAsync()
    {
        var buffer = await _fileReader.ReadBytesAsync(ShortSize);
        return BinaryPrimitives.ReadInt16LittleEndian(buffer);
    }

    public async ValueTask<int> ReadIntAsync()
    {
        var buffer = await _fileReader.ReadBytesAsync(IntSize);
        return BinaryPrimitives.ReadInt32LittleEndian(buffer);
    }

    public async ValueTask<float> ReadFloatAsync()
    {
        var buffer = await _fileReader.ReadBytesAsync(FloatSize);
        return BinaryPrimitives.ReadSingleLittleEndian(buffer);
    }

    public async ValueTask<double> ReadDoubleAsync()
    {
        var buffer = await _fileReader.ReadBytesAsync(DoubleSize);
        return BinaryPrimitives.ReadDoubleLittleEndian(buffer);
    }

    public async ValueTask<Gp5Bool> ReadBoolAsync() =>
        (Gp5Bool)await ReadByteValueAsync();

    public async ValueTask<Gp5Color> ReadColorAsync() =>
        (Gp5Color)await _fileReader.ReadBytesAsync(IntSize);

    private async ValueTask<byte> ReadByteValueAsync()
    {
        var buffer = await _fileReader.ReadBytesAsync(ByteSize);
        return buffer[0];
    }
}
