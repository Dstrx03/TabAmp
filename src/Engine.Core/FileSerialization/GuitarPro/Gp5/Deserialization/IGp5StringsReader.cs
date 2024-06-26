﻿using System.Threading.Tasks;
using TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Models.Strings;

namespace TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Deserialization;

internal interface IGp5StringsReader
{
    ValueTask<Gp5ByteString> ReadByteStringAsync(int maxLength);
    ValueTask<string> ReadIntStringAsync();
    ValueTask<Gp5IntByteString> ReadIntByteStringAsync();
}
