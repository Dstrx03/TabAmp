using System;
using TabAmp.Engine.Core.FileSerialization.Common.Components.Metadata;

namespace TabAmp.Engine.Core.FileSerialization.Common.Components.Processor;

internal interface IExactFileDeserializer : IFileDeserializationMetadataProvider
{
    void Todo_Name()
    {
        if (Metadata.TODO_length is null || Metadata.TODO_position is null)
            throw new Exception("TODO: metadata is null");

        if (Metadata.TODO_length != Metadata.TODO_position)
            throw new Exception("TODO: not fully read");
    }
}
