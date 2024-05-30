using System.Threading.Tasks;
using TabAmp.Engine.Core.FileSerialization.Common.Exceptions;

namespace TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Deserialization;

internal class Gp5BinaryPrimitivesReaderIntegrityValidator : IGp5BinaryPrimitivesReader
{
    private readonly IGp5BinaryPrimitivesReader _primitivesReader;

    public Gp5BinaryPrimitivesReaderIntegrityValidator(IGp5BinaryPrimitivesReader primitivesReader) =>
        _primitivesReader = primitivesReader;

    public ValueTask<byte> ReadByteAsync() =>
        _primitivesReader.ReadByteAsync();

    public ValueTask<sbyte> ReadSignedByteAsync() =>
        _primitivesReader.ReadSignedByteAsync();

    public ValueTask<short> ReadShortAsync() =>
        _primitivesReader.ReadShortAsync();

    public ValueTask<int> ReadIntAsync() =>
        _primitivesReader.ReadIntAsync();

    public ValueTask<float> ReadFloatAsync() =>
        _primitivesReader.ReadFloatAsync();

    public ValueTask<double> ReadDoubleAsync() =>
        _primitivesReader.ReadDoubleAsync();

    public async ValueTask<Gp5Bool> ReadBoolAsync()
    {
        var boolValue = await _primitivesReader.ReadBoolAsync();

        if (boolValue.ByteValue is not Gp5Bool.FalseValue and not Gp5Bool.TrueValue)
            // TODO: message
            throw new FileSerializationIntegrityException($"{boolValue.ByteValue}!=0<>1 P=~");

        return boolValue;
    }

    public async ValueTask<Gp5Color> ReadColorAsync()
    {
        var colorValue = await _primitivesReader.ReadColorAsync();

        if (colorValue._A01 != 0)
            // TODO: message
            throw new FileSerializationIntegrityException($"{colorValue._A01}!=0 P=~");

        return colorValue;
    }
}
