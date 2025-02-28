using System.Threading.Tasks;
using TabAmp.Engine.Core.FileSerialization.Common.Exceptions.IntegrityValidation;
using TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Models.BinaryPrimitives;

namespace TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Deserialization.IntegrityValidators;

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
        var boolWrapper = await _primitivesReader.ReadBoolAsync();

        if (boolWrapper.Value is not Gp5Bool.FalseValue and not Gp5Bool.TrueValue)
        {
            var message = $"todo: message {boolWrapper.Value}";
            throw new ProcessIntegrityException(message);
        }

        return boolWrapper;
    }

    public async ValueTask<Gp5Color> ReadColorAsync()
    {
        var colorWrapper = await _primitivesReader.ReadColorAsync();

        if (colorWrapper._A01 != 0)
            // TODO: message
            throw new ProcessIntegrityException($"{colorWrapper._A01}!=0 P=~");

        return colorWrapper;
    }
}
