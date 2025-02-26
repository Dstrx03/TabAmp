using System.Threading.Tasks;
using TabAmp.Engine.Core.FileSerialization.Common.Exceptions.IntegrityValidation;
using TabAmp.Engine.Core.FileSerialization.Common.Exceptions.IO;
using TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Models.Text;

namespace TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Deserialization.IntegrityValidators;

internal class Gp5TextReaderIntegrityValidator : IGp5TextReader
{
    private readonly IGp5TextReader _textReader;

    public Gp5TextReaderIntegrityValidator(IGp5TextReader textReader) =>
        _textReader = textReader;

    public async ValueTask<Gp5ByteText> ReadByteTextAsync(int maxLength)
    {
        try
        {
            return await _textReader.ReadByteTextAsync(maxLength);
        }
        catch (NegativeBytesCountOperationException exception) when (exception.Operation == OperationType.ReadSkip)
        {
            var length = maxLength + exception.BytesCount * -1;
            var message = $"The text length ({length}) exceeds the maximum length of {maxLength}.";
            throw new ProcessIntegrityException(message, exception);
        }
    }

    public async ValueTask<string> ReadIntTextAsync()
    {
        try
        {
            return await _textReader.ReadIntTextAsync();
        }
        catch (NegativeBytesCountOperationException exception) when (exception.Operation == OperationType.Read)
        {
            var length = exception.BytesCount;
            var message = $"The text length ({length}) must be a non-negative number.";
            throw new ProcessIntegrityException(message, exception);
        }
    }

    public async ValueTask<Gp5IntByteText> ReadIntByteTextAsync()
    {
        var textWrapper = await _textReader.ReadIntByteTextAsync();

        if (textWrapper.Length != textWrapper.MaxLength)
            // TODO: message
            throw new ProcessIntegrityException($"{textWrapper.Length}+1!={textWrapper.Size} P=~");

        return textWrapper;
    }
}
