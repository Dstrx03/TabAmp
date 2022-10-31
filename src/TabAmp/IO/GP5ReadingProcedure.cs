using TabAmp.Models;

namespace TabAmp.IO;

public class GP5ReadingProcedure : ITabFileReadingProcedure
{
    private readonly ITabFileReaderContext _context;

    public GP5ReadingProcedure(ITabFileReaderContext context) =>
        _context = context;

    public Task<Song> ReadAsync()
    {
        var song = new Song();
        song.Version = $"v_{_context.Path}";
        return Task.FromResult(song);
    }
}
