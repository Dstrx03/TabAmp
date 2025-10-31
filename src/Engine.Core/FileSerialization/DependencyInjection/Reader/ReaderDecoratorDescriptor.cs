using TabAmp.Shared.Decorator.Fluent;

namespace TabAmp.Engine.Core.FileSerialization.DependencyInjection.Reader;

internal abstract record ReaderDecoratorDescriptor<TService, TReader>
    where TService : notnull
    where TReader : notnull, TService
{
    public abstract ServiceDecoratorDescriptorChainFluentBuilder<TService, TReader> AppendTo(
        ServiceDecoratorDescriptorChainFluentBuilder<TService, TReader> builder);
}
