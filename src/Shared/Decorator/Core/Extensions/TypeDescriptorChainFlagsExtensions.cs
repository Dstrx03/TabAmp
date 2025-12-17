using System;
using TabAmp.Shared.Decorator.Core.DescriptorChain;

namespace TabAmp.Shared.Decorator.Core.Extensions;

internal static class TypeDescriptorChainFlagsExtensions
{
    internal static ServiceDecoratorDescriptorChainFlags ToDescriptorChainFlags<TService>(
        this Type decoratorType,
        ServiceDecoratorDescriptorChain<TService>? next)
        where TService : notnull
    {
        var isDecoratorDisposable = decoratorType.IsDisposable();
        var isDecoratorAsyncDisposable = decoratorType.IsAsyncDisposable();
        var decoratorRequiresDisposal = isDecoratorDisposable || isDecoratorAsyncDisposable;
        var anyDecoratorRequiresDisposal = decoratorRequiresDisposal || (next?.AnyDecoratorRequiresDisposal ?? false);

        ServiceDecoratorDescriptorChainFlags flags = new();

        if (isDecoratorDisposable) flags |= ServiceDecoratorDescriptorChainFlags.IsDecoratorDisposable;
        if (isDecoratorAsyncDisposable) flags |= ServiceDecoratorDescriptorChainFlags.IsDecoratorAsyncDisposable;
        if (anyDecoratorRequiresDisposal) flags |= ServiceDecoratorDescriptorChainFlags.AnyDecoratorRequiresDisposal;

        return flags;
    }
}
