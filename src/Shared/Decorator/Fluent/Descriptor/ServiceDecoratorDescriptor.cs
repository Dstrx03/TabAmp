namespace TabAmp.Shared.Decorator.Fluent.Descriptor;

internal interface IServiceDecoratorDescriptor<TService>
{
    IServiceDecoratorDescriptorNode<TService> ToNode(IServiceDecoratorDescriptorNode<TService>? next);
}

internal sealed record ServiceDecoratorDescriptor<TService, TDecorator> : IServiceDecoratorDescriptor<TService>
    where TService : notnull
    where TDecorator : notnull, TService
{
    public IServiceDecoratorDescriptorNode<TService> ToNode(IServiceDecoratorDescriptorNode<TService>? next) =>
        new ServiceDecoratorDescriptorNode<TService, TDecorator>(next);
}
