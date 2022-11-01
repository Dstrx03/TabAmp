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

    public Task<sbyte> ReadNextSignedByteAsync() =>
        ReadNextSignedByteAsync();

    public Task<bool> ReadNextBoolAsync()
    {
        throw new NotImplementedException();
    }

    public Task<short> ReadNextShortAsync()
    {
        throw new NotImplementedException();
    }

    public Task<int> ReadNextIntAsync()
    {
        throw new NotImplementedException();
    }

    public Task<float> ReadNextFloatAsync()
    {
        throw new NotImplementedException();
    }

    public Task<double> ReadNextDoubleAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<string> ReadNextByteSizeStringAsync()
    {
        var size = await ReadNextByteAsync();
        return await ReadNextStringAsync(size);
    }

    public async Task<string> ReadNextIntSizeStringAsync()
    {
        var size = await ReadNextIntAsync();
        return await ReadNextStringAsync(size);
    }

    public Task<string> ReadNextIntByteSizeStringAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<string> ReadNextStringAsync(int size)
    {
        var memory = await _streamReader.ReadNextBytesAsync(size);
        return Encoding.UTF8.GetString(memory.Span);
    }
}
