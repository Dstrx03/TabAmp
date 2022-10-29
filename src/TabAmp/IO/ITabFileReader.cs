using TabAmp.Models;

namespace TabAmp.IO
{
    public interface ITabFileReader
    {
        public Task<Song> ReadAsync(string path, CancellationToken cancellationToken);
    }
}
