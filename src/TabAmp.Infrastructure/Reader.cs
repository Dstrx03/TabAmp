namespace TabAmp.Infrastructure
{
    public class Reader : IDisposable
    {
        private readonly FileStream _stream;

        public Reader()
        {
            _stream = File.Open("", FileMode.Open);
        }

        public void Dispose()
        {
            ((IDisposable)_stream).Dispose();
        }
    }
}
