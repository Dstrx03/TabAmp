using TabAmp.Shared.Decorator.Fluent.Descriptor;

namespace TabAmp.Engine.Core.FileSerialization.DependencyInjection.Reader;

internal interface IIntegrityValidatorDescriptor<TService> : IServiceDecoratorDescriptorHost<TService>
    where TService : class
{
    public sealed class For<TIntegrityValidator> :
        ServiceDecoratorDescriptor<TService>.For<TIntegrityValidator>,
        IIntegrityValidatorDescriptor<TService>
        where TIntegrityValidator : class, TService
    {
        public ServiceDecoratorDescriptor<TService> Descriptor => this;
    }
}
