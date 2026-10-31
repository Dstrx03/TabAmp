using TabAmp.Shared.Decorator.Fluent.Descriptor;

namespace TabAmp.Engine.Core.FileSerialization.DependencyInjection.Reader;

internal interface IIntegrityValidatorDescriptor<TService> : IServiceDecoratorDescriptorHost<TService>
    where TService : class
{
}
