using System.Threading.Tasks;
using TabAmp.Engine.Core.FileSerialization.Common.Components.SerialFileReader;
using TabAmp.Engine.Core.FileSerialization.Common.Exceptions;
using TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Models.Wrappers;

namespace TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Deserialization;

internal class Gp5FileReaderIntegrityValidator : Gp5FileReader
{
    public Gp5FileReaderIntegrityValidator(ISerialFileReader fileReader)
        : base(fileReader)
    {
    }

    protected override async ValueTask<Gp5Boolean> ReadBooleanAsync()
    {
        var result = await base.ReadBooleanAsync();

        if (result.ByteValue is not Gp5Boolean.FalseValue and not Gp5Boolean.TrueValue)
            throw new FileSerializationIntegrityException($"{nameof(Gp5Boolean)} value of {result.ByteValue} cannot have values other than {Gp5Boolean.FalseValue} or {Gp5Boolean.TrueValue}.");

        return result;
    }
}
