using System.Text;

namespace TabAmp.IO;

public class GP5BasicTypesReader
{
	private readonly ISequentialStreamReader _streamReader;

	public GP5BasicTypesReader(ISequentialStreamReader streamReader) =>
		_streamReader = streamReader;

	public async Task<byte> ReadNextByteAsync()
	{
		var buffer = await _streamReader.ReadNextBytesAsync(1);
		return buffer.Span[0];
	}

	public async Task<string> ReadNextByteSizeStringAsync()
	{
		var size = await ReadNextByteAsync();
		var stringBytes = await _streamReader.ReadNextBytesAsync(size);
		return Encoding.UTF8.GetString(stringBytes.Span);
	}
}
