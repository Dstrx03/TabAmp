using System;

namespace TabAmp.Shared.Decorator.Core.DescriptorChain;

[Flags]
internal enum ServiceDecoratorDescriptorChainFlags : byte
{
    IsDecoratorDisposable = 0x20,
    HasInnerDisposableDecorator = 0x40,
    IsDisposableContainerAllowed = 0x08

    /*
    A = 0x01,
    B = 0x02,
    C = 0x04,
    D = 0x08,
    E = 0x10,
    F = 0x20,
    G = 0x40
    */
}
