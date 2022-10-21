using System.Text;
using TabAmp.Models;

namespace TabAmp.IO
{
    public class TabFileReader
    {
        private readonly Reader _reader;
        private readonly TabFileTypesReader _typesReader;

        public TabFileReader(Reader reader, TabFileTypesReader typesReader) =>
            (_reader, _typesReader) = (reader, typesReader);

        public async Task<Song> ReadAsync(string path)
        {
            _reader.Open(path);
            var song = new Song();
            await ReadVersionAsync(song);
            return song;
        }

        private async Task ReadVersionAsync(Song song)
        {
            _reader.SkipBytesSequence(2);
            var versionBytes = new byte[4];
            for (var i = 0; i < 4; i++)
                versionBytes[i] = await _typesReader.ReadByteAsync();
            var versionString = Encoding.UTF8.GetString(versionBytes);
            song.Version = versionString;
        }
    }
}
