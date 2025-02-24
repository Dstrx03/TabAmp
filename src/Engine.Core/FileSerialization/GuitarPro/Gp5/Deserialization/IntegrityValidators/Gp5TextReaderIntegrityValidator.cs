using System.Threading.Tasks;
using TabAmp.Engine.Core.FileSerialization.Common.Exceptions.IntegrityValidation;
using TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Models.Text;

namespace TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Deserialization.IntegrityValidators;

internal class Gp5TextReaderIntegrityValidator : IGp5TextReader
{
    private readonly IGp5TextReader _textReader;

    public Gp5TextReaderIntegrityValidator(IGp5TextReader textReader) =>
        _textReader = textReader;

    public async ValueTask<Gp5ByteText> ReadByteTextAsync(int maxLength)
    {
        var textWrapper = await _textReader.ReadByteTextAsync(maxLength);

        if (textWrapper.TrailingBytesCount < 0)
            // TODO: message
            throw new ProcessIntegrityException($"{maxLength}-{textWrapper.Length}<0 P=~");

        return textWrapper;
    }

    public ValueTask<string> ReadIntTextAsync() =>
        _textReader.ReadIntTextAsync();

    public async ValueTask<Gp5IntByteText> ReadIntByteTextAsync()
    {
        var textWrapper = await _textReader.ReadIntByteTextAsync();

        if (textWrapper.Length != textWrapper.MaxLength)
            // TODO: message
            throw new ProcessIntegrityException($"{textWrapper.Length}+1!={textWrapper.Size} P=~");

        return textWrapper;
    }
}
