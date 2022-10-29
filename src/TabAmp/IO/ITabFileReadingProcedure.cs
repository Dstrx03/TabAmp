using TabAmp.Models;

namespace TabAmp.IO
{
    public interface ITabFileReadingProcedure
    {
        public Task<Song> ReadAsync();
    }
}
