using System;
using TabAmp.Shared.Decorator.Core.DescriptorChain;

namespace TabAmp.Shared.Decorator.Core.Extensions;

internal static class ServiceDecoratorDescriptorChainOptionsExtensions
{
    internal static object? GetImplementationServiceKey(
        this ServiceDecoratorDescriptorChainOptions options,
        object? implementationServiceKey,
        object? defaultImplementationServiceKey)
    {
        if (!options.HasFlag(ServiceDecoratorDescriptorChainOptions.UseStandaloneImplementationService))
            return null;

        if (options.HasFlag(ServiceDecoratorDescriptorChainOptions.UseDefaultImplementationServiceKey))
            return defaultImplementationServiceKey;

        ArgumentNullException.ThrowIfNull(implementationServiceKey);

        return implementationServiceKey;
    }
}
