using TabAmp.Models;

namespace TabAmp.Commands
{
    public class ReadTabFileResult
    {
        public ReadTabFileResult(TabFile tabFile)
        {
            Success = true;
            TabFile = tabFile;
        }

        public ReadTabFileResult(Exception exception)
        {
            Success = false;
            Exception = exception;
        }

        public TabFile TabFile { get; }
        public bool Success { get; }
        public Exception Exception { get; }
    }
}
