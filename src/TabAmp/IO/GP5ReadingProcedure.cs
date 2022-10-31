using TabAmp.Models;

namespace TabAmp.IO;

public class GP5ReadingProcedure : ITabFileReadingProcedure
{
    private readonly GP5BasicTypesReader _reader;

    public GP5ReadingProcedure(GP5BasicTypesReader reader) =>
        _reader = reader;

    public Task<Song> ReadAsync()
    {
        var song = new Song();
        song.Version = $"v_{_reader.ReadNextString()}";
        return Task.FromResult(song);
    }
}
