namespace TabAmp.IO;

internal class TabFileExtensionNotSupportedException : TabFileReaderException
{
	private const string MessageTemplate = "{0} filename extension is not supported.";

	public TabFileExtensionNotSupportedException(string filePath)
		: base(string.Format(MessageTemplate, filePath)) { }
}
