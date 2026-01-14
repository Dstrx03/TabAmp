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
}
