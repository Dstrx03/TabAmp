namespace TabAmp.Shared.Decorator.Fluent.Descriptor;

internal interface IServiceDecoratorDescriptorHost<TService>
    where TService : class
{
    ServiceDecoratorDescriptor<TService> Descriptor { get; }
}
