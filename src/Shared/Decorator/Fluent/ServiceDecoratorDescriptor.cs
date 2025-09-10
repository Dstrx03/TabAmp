namespace TabAmp.Shared.Decorator.Fluent;

public abstract record ServiceDecoratorDescriptor
{
    internal abstract ServiceDecoratorDescriptorNode ToNode(ServiceDecoratorDescriptorNode? next);

    internal sealed record Instance<TDecorator> : ServiceDecoratorDescriptor
    {
        internal override ServiceDecoratorDescriptorNode ToNode(ServiceDecoratorDescriptorNode? next) =>
            new ServiceDecoratorDescriptorNode.Instance<TDecorator>(next);
    }
}
