using TabAmp.Shared.Decorator.Fluent;

namespace TabAmp.Engine.Core.FileSerialization.DependencyInjection.Reader;

public abstract record IntegrityValidatorDescriptor<TService>
    where TService : notnull
{
    public abstract void AppendTo(ServiceDecoratorDescriptorChainFluentBuilder<TService> builder,
        out ServiceDecoratorDescriptorChainFluentBuilder<TService> nextBuilder);

    internal sealed record For<TIntegrityValidator> : IntegrityValidatorDescriptor<TService>
        where TIntegrityValidator : notnull, TService
    {
        public override void AppendTo(ServiceDecoratorDescriptorChainFluentBuilder<TService> builder,
            out ServiceDecoratorDescriptorChainFluentBuilder<TService> nextBuilder)
        {
            nextBuilder = builder.With<TIntegrityValidator>();
        }
    }
}
