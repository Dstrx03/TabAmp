using TabAmp.Models;

namespace TabAmp.IO;

public class GP5ReadingProcedure : ITabFileReadingProcedure
{
    private readonly GP5BasicTypesReader _reader;

    public GP5ReadingProcedure(GP5BasicTypesReader reader) =>
        _reader = reader;

    public async Task<TabFile> ReadAsync()
    {
        var tabFile = new TabFile();
        tabFile.Version = await _reader.ReadNextByteSizeStringAsync();
        return tabFile;
    }
}
