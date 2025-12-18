using System;

namespace TabAmp.Shared.Decorator.Core.DescriptorChain;

[Flags]
internal enum ServiceDecoratorDescriptorChainOptions : byte
{
    UseStandaloneImplementationService = 0x10,
    UseDefaultImplementationServiceKey = 0x20,
    AllowA = 0x40
}
