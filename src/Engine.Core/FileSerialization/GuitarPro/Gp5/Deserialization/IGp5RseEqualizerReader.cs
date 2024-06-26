﻿using System.Threading.Tasks;
using TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Models;

namespace TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Deserialization;

internal interface IGp5RseEqualizerReader
{
    ValueTask<Gp5RseEqualizer> ReadRseEqualizerAsync(int bandsCount);
}
