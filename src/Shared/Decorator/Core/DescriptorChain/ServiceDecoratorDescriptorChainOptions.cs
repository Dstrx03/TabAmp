using System;

namespace TabAmp.Shared.Decorator.Core.DescriptorChain;

[Flags]
internal enum ServiceDecoratorDescriptorChainOptions : byte
{
    UseDefaultImplementationServiceKey = 0x01,
    IsDisposableContainerAllowed = 0x02
}
