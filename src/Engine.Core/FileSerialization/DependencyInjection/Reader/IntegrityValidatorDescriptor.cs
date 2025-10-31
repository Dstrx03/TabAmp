using TabAmp.Shared.Decorator.Fluent;

namespace TabAmp.Engine.Core.FileSerialization.DependencyInjection.Reader;

internal abstract record IntegrityValidatorDescriptor<TService, TReader> : ReaderDecoratorDescriptor<TService, TReader>
    where TService : notnull
    where TReader : notnull, TService
{
    public sealed record For<TIntegrityValidator> : IntegrityValidatorDescriptor<TService, TReader>
        where TIntegrityValidator : notnull, TService
    {
        public override ServiceDecoratorDescriptorChainFluentBuilder<TService, TReader> AppendTo(
            ServiceDecoratorDescriptorChainFluentBuilder<TService, TReader> builder)
        {
            return builder.With<TIntegrityValidator>();
        }
    }
}
