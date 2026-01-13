using System;

namespace TabAmp.Shared.Decorator.Core.DescriptorChain;

[Flags]
internal enum ServiceDecoratorDescriptorChainFlags : byte
{
    IsServiceDisposable = 0x01,
    IsServiceAsyncDisposable = 0x02,
    IsDecoratorDisposable = 0x04,
    IsDecoratorAsyncDisposable = 0x08,
    IsDisposableContainerAllowed = 0x10
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
