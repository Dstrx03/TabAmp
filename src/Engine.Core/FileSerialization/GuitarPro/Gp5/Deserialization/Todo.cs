using System;
using System.Threading.Tasks;
using TabAmp.Engine.Core.FileSerialization.Common.Components.SerialFileReader;
using TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Models.Wrappers;

namespace TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Deserialization;

internal class Todo : Gp5FileReader // TODO: name!
{
    public Todo(ISerialFileReader fileReader) : base(fileReader)
    {
    }

    protected override async ValueTask<Gp5Boolean> ReadBooleanAsync()
    {
        var value = await base.ReadBooleanAsync();

        if (value.ByteValue is not Gp5Boolean.FalseValue and not Gp5Boolean.TrueValue)
            // TODO: more specific exception type, message
            throw new InvalidOperationException($"{value.ByteValue}!=0<>1 P=_fileReader.Position");

        return value;
    }
}
