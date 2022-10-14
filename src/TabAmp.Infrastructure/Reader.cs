namespace TabAmp.Infrastructure
{
    public class Reader : IDisposable
    {
        private readonly FileStream _stream;

        public Reader(string path)
        {
            _stream = File.Open(path, FileMode.Open);
        }

        public async Task ReadBytesAsync(Memory<byte> buffer, CancellationToken cancellationToken)
        {
            await _stream.ReadAsync(buffer, cancellationToken);
        }

        public void Dispose()
        {
            ((IDisposable)_stream).Dispose();
        }

        [Obsolete]
        public async Task<byte[]> ReadBytesAsync(int count, CancellationToken cancellationToken)
        {
            var buffer = new byte[count];
            await _stream.ReadAsync(buffer);
            return buffer;
        }
    }
}
