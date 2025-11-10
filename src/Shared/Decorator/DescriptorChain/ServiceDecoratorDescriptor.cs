using System;

namespace TabAmp.Shared.Decorator.DescriptorChain;

public abstract class ServiceDecoratorDescriptor<TService>
    where TService : notnull
{
    internal ServiceDecoratorDescriptor<TService>? Next { get; private set; }
    internal int? Position { get; private set; }

    private ServiceDecoratorDescriptor()
    {
    }

    public bool IsAppended => Position is not null;

    internal ServiceDecoratorDescriptor<TService> AppendTo(ServiceDecoratorDescriptor<TService>? descriptors)
    {
        if (IsAppended) throw A();
        if (descriptors?.IsAppended == false) throw B();
        if (descriptors?.Position >= 10) throw C();
        Next = descriptors;
        Position = descriptors?.Position + 1 ?? 1;
        return this;
    }

    internal ServiceDecoratorDescriptorChain<TService> ToChain(ServiceDecoratorDescriptorChain<TService> descriptorChain)
    {
        if (!IsAppended) throw D();
        return CreateDescriptorChainNode(descriptorChain);
    }

    private protected abstract ServiceDecoratorDescriptorChain<TService> CreateDescriptorChainNode(
        ServiceDecoratorDescriptorChain<TService> descriptorChain);

    public class For<TDecorator> : ServiceDecoratorDescriptor<TService>
        where TDecorator : notnull, TService
    {
        private protected sealed override ServiceDecoratorDescriptorChain<TService> CreateDescriptorChainNode(
            ServiceDecoratorDescriptorChain<TService> descriptorChain)
        {
            return new ServiceDecoratorDescriptorChain<TService>.For<TDecorator>(next: descriptorChain);
        }
    }
    private static InvalidOperationException A() => new("TODO: message A");
    private static InvalidOperationException B() => new("TODO: message B");
    private static InvalidOperationException C() => new("TODO: message C");
    private static InvalidOperationException D() => new("TODO: message D");
    //private static InvalidOperationException ContextAlreadyExistsException() =>
    //    new($"Cannot create the context: {nameof(FileSerializationContext)} already exists in the current scope " +
    //        $"with {nameof(FileSerializationContext.FilePath)}: '{context.FilePath}' and cannot be initialized again.");
}
