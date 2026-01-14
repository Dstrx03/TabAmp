using System;

namespace TabAmp.Shared.Decorator.Core.DescriptorChain;

[Flags]
internal enum ServiceDecoratorDescriptorChainOptions : byte
{
    IsDisposableContainerAllowed = 0x01,
    UseDefaultImplementationServiceKey = 0x02
}
