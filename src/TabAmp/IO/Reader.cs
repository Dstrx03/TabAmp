namespace TabAmp.IO
{
    public class Reader : IDisposable
    {
        private FileStream _fileStream;

        public void Open(string path)
        {
            var options = new FileStreamOptions
            {
                Options = FileOptions.Asynchronous,
                BufferSize = 0,
                Share = FileShare.None
            };
            _fileStream = File.Open(path, options);
        }

        public void Dispose()
        {
            _fileStream?.Dispose();
        }

        public async ValueTask<ReadOnlyMemory<byte>> ReadBytesSequenceAsync(int count)
        {
            var buffer = new byte[count];
            await _fileStream.ReadAsync(buffer);
            return buffer;
        }

        public void SkipBytesSequence(int count) =>
            _fileStream.Position += count;
    }
}
