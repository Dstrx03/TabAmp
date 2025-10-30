using TabAmp.Shared.Decorator.Fluent;

namespace TabAmp.Engine.Core.FileSerialization.DependencyInjection.Reader;

public abstract record IntegrityValidatorDescriptor<TService, TReader>
    where TService : notnull
    where TReader : notnull, TService
{
    public abstract void AppendTo(ServiceDecoratorDescriptorChainFluentBuilder<TService, TReader> builder,
        out ServiceDecoratorDescriptorChainFluentBuilder<TService, TReader> nextBuilder);

    internal sealed record For<TIntegrityValidator> : IntegrityValidatorDescriptor<TService, TReader>
        where TIntegrityValidator : notnull, TService
    {
        public override void AppendTo(ServiceDecoratorDescriptorChainFluentBuilder<TService, TReader> builder,
            out ServiceDecoratorDescriptorChainFluentBuilder<TService, TReader> nextBuilder)
        {
            nextBuilder = builder.With<TIntegrityValidator>();
        }
    }
}
