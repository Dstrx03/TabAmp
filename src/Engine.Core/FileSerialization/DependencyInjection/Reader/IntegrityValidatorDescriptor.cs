using TabAmp.Shared.Decorator.Fluent;

namespace TabAmp.Engine.Core.FileSerialization.DependencyInjection.Reader;

public abstract record IntegrityValidatorDescriptor<TService>
    where TService : notnull
{
    public abstract void Append(ServiceDecoratorDescriptorChainFluentBuilder<TService> builder,
        out ServiceDecoratorDescriptorChainFluentBuilder<TService> appended);

    internal sealed record For<TIntegrityValidator> : IntegrityValidatorDescriptor<TService>
        where TIntegrityValidator : notnull, TService
    {
        public override void Append(ServiceDecoratorDescriptorChainFluentBuilder<TService> builder,
            out ServiceDecoratorDescriptorChainFluentBuilder<TService> appended)
        {
            appended = builder.With<TIntegrityValidator>();
        }
    }
}
