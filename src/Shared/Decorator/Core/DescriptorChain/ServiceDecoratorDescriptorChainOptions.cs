using System;

namespace TabAmp.Shared.Decorator.Core.DescriptorChain;

[Flags]
internal enum ServiceDecoratorDescriptorChainOptions : byte
{
    UseDefaultImplementationServiceKey = 0x20,
    AllowA = 0x40
}
