namespace TabAmp.Infrastructure
{
    public class Reader : IDisposable
    {
        private readonly FileStream _stream;

        public Reader()
        {
            _stream = File.Open("../../../../../file.gp5", FileMode.Open);
        }

        public async Task<byte[]> ReadBytesAsync()
        {
            var buffer = new byte[_stream.Length];
            await _stream.ReadAsync(buffer);
            return buffer;
        }

        public void Dispose()
        {
            ((IDisposable)_stream).Dispose();
        }
    }
}
