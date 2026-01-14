using Options = TabAmp.Shared.Decorator.Core.DescriptorChain.ServiceDecoratorDescriptorChainOptions;

namespace TabAmp.Shared.Decorator.Core.Extensions;

internal static class ServiceDecoratorDescriptorChainOptionsExtensions
{
    internal static bool HasOption(this Options options, Options option) => (options & option) == option;
}
