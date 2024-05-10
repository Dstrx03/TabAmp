using System.Threading.Tasks;

namespace TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Deserialization;

internal interface IGp5BinaryPrimitivesReader
{
    ValueTask<byte> ReadByteAsync();
    ValueTask<sbyte> ReadSignedByteAsync();
    ValueTask<short> ReadShortAsync();
    ValueTask<int> ReadIntAsync();
    ValueTask<float> ReadFloatAsync();
    ValueTask<double> ReadDoubleAsync();
    ValueTask<bool> ReadBoolAsync();
}
