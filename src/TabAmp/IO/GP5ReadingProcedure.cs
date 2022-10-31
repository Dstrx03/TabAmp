using TabAmp.Models;

namespace TabAmp.IO
{
    public class GP5ReadingProcedure : ITabFileReadingProcedure
    {
        private readonly TabFileReaderContext _context;

        public GP5ReadingProcedure(TabFileReaderContext context) =>
            _context = context;

        public Task<Song> ReadAsync()
        {
            _context.Path += "+mutated";
            var song = new Song();
            song.Version = $"v_{_context.Path}";
            return Task.FromResult(song);
        }
    }
}
