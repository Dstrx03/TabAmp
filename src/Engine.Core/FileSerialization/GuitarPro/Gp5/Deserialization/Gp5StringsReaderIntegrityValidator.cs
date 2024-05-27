using System.Threading.Tasks;

namespace TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Deserialization;

internal class Gp5StringsReaderIntegrityValidator : IGp5StringsReader
{
    private readonly IGp5StringsReader _stringsReader;

    public Gp5StringsReaderIntegrityValidator(IGp5StringsReader stringsReader) =>
        _stringsReader = stringsReader;

    public async ValueTask<string> ReadByteStringAsync(int maxLength)
    {
        return await _stringsReader.ReadByteStringAsync(maxLength);
    }

    public async ValueTask<string> ReadIntStringAsync()
    {
        return await _stringsReader.ReadIntStringAsync();
    }

    public async ValueTask<string> ReadIntByteStringAsync()
    {
        return await _stringsReader.ReadIntByteStringAsync();
    }
}
