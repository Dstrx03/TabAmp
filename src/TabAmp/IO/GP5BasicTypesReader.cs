using System.Buffers.Binary;
using System.Text;

namespace TabAmp.IO;

public class GP5BasicTypesReader
{
    private readonly ISequentialStreamReader _streamReader;

    public GP5BasicTypesReader(ISequentialStreamReader streamReader) =>
        _streamReader = streamReader;

    public async Task<byte> ReadNextByteAsync()
    {
        var memory = await _streamReader.ReadNextBytesAsync(1);
        return memory.Span[0];
    }

    public async Task<sbyte> ReadNextSignedByteAsync()
    {
        var value = await ReadNextByteAsync();
        return (sbyte)value;
    }

    public async Task<bool> ReadNextBoolAsync()
    {
        var value = await ReadNextByteAsync();
        return value == 1;
    }

    public async Task<short> ReadNextShortAsync()
    {
        var memory = await _streamReader.ReadNextBytesAsync(2);
        return BinaryPrimitives.ReadInt16LittleEndian(memory.Span);
    }

    public async Task<int> ReadNextIntAsync()
    {
        var memory = await _streamReader.ReadNextBytesAsync(4);
        return BinaryPrimitives.ReadInt32LittleEndian(memory.Span);
    }

    public async Task<float> ReadNextFloatAsync()
    {
        var memory = await _streamReader.ReadNextBytesAsync(4);
        return BinaryPrimitives.ReadSingleLittleEndian(memory.Span);
    }

    public async Task<double> ReadNextDoubleAsync()
    {
        var memory = await _streamReader.ReadNextBytesAsync(8);
        return BinaryPrimitives.ReadDoubleLittleEndian(memory.Span);
    }

    public async Task<string> ReadNextByteSizeStringAsync(int readSize = 0)
    {
        var size = await ReadNextByteAsync();
        return await ReadNextStringAsync(size, readSize);
    }

    public async Task<string> ReadNextIntSizeStringAsync(int readSize = 0)
    {
        var size = await ReadNextIntAsync();
        return await ReadNextStringAsync(size, readSize);
    }

    public async Task<string> ReadNextIntByteSizeStringAsync()
    {
        var intValue = await ReadNextIntAsync();
        var byteValue = await ReadNextByteAsync();
        var readSize = intValue - 1;
        var stringValue = await ReadNextStringAsync(byteValue, readSize);
        return stringValue;
    }

    private async Task<string> ReadNextStringAsync(int size, int readSize)
    {
        var memory = await _streamReader.ReadNextBytesAsync(size);
        var skipBytes = readSize - size;
        if (skipBytes > 0)
            _streamReader.SkipNextBytes(skipBytes);
        return Encoding.UTF8.GetString(memory.Span);
    }
}
