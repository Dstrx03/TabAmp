using System;
using TabAmp.Engine.Core.FileSerialization.Common.Components.Metadata;

namespace TabAmp.Engine.Core.FileSerialization.Common.Components.Processor;

internal interface IExactFileDeserializer : IFileDeserializationMetadataProvider
{
    void Todo_Name()
    {
        if (Metadata.Length is null || Metadata.ProcessedBytes is null)
            throw new Exception("TODO: metadata is null");

        if (Metadata.Length != Metadata.ProcessedBytes)
            throw new Exception("TODO: not fully read");
    }
}
