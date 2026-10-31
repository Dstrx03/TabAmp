namespace TabAmp.Shared.Decorator.Fluent.Descriptor;

public interface IServiceDecoratorDescriptorHost<TService>
    where TService : class
{
    ServiceDecoratorDescriptor<TService> Descriptor { get; }
}
