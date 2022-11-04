namespace TabAmp.IO;

internal class TabFileReaderException : Exception
{
    public TabFileReaderException() { }

    public TabFileReaderException(string message)
        : base(message) { }

    public TabFileReaderException(string message, Exception inner)
        : base(message, inner) { }

    public TabFileReaderException(Exception inner)
        : base(inner.Message, inner) { }
}
