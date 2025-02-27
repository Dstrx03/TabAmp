﻿using System.Threading.Tasks;
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
            var message = $"The text length exceeds the maximum length of {maxLength}. Actual length: {length}";
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
            var message = $"The text length must be a non-negative number. Actual length: {length}";
            throw new ProcessIntegrityException(message, exception);
        }
    }

    public async ValueTask<Gp5IntByteText> ReadIntByteTextAsync()
    {
        var textWrapper = await _textReader.ReadIntByteTextAsync();

        var size = Gp5IntByteText.CalculateSize(textWrapper);
        if (textWrapper.Size != size )
        {
            var message = $"The expected size for a text with length of {textWrapper.Length} is {size} byte(s). Actual size: {textWrapper.Size} byte(s)";
            throw new ProcessIntegrityException(message);
        }

        return textWrapper;
    }
}
