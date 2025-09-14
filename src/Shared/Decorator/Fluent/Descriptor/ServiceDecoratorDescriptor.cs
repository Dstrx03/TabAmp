namespace TabAmp.Shared.Decorator.Fluent.Descriptor;

public abstract record ServiceDecoratorDescriptor<TService>
    where TService : notnull
{
    internal abstract ServiceDecoratorDescriptorNode<TService> ToNode(ServiceDecoratorDescriptorNode<TService>? next);

    internal sealed record Instance<TDecorator> : ServiceDecoratorDescriptor<TService>
        where TDecorator : notnull, TService
    {
        internal override ServiceDecoratorDescriptorNode<TService> ToNode(ServiceDecoratorDescriptorNode<TService>? next) =>
            new ServiceDecoratorDescriptorNode<TService>.Instance<TDecorator>(next);
    }
}
