using TabAmp.Models;

namespace TabAmp.Commands
{
    public class ReadTabFileResult
    {
        public ReadTabFileResult(Song song)
        {
            Success = true;
            Song = song;
        }

        public ReadTabFileResult(Exception exception)
        {
            Success = false;
            Exception = exception;
        }

        public Song Song { get; }
        public bool Success { get; }
        public Exception Exception { get; }
    }
}
