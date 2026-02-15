using System;
using TabAmp.Shared.Decorator.Core.Activators;
using TabAmp.Shared.Decorator.Core.Extensions;
using Options = TabAmp.Shared.Decorator.Core.DescriptorChain.ServiceDecoratorDescriptorChainOptions;

namespace TabAmp.Shared.Decorator.Core.DescriptorChain;

public abstract class ServiceDecoratorDescriptorChain<TService, TImplementation>
    where TService : class
    where TImplementation : class, TService
{
    private readonly ChainFlags _chainFlags;
    private readonly DescriptorFlags _descriptorFlags;

    public ServiceDecoratorDescriptorChain<TService, TImplementation>? Next { get; }

    private ServiceDecoratorDescriptorChain(
        ServiceDecoratorDescriptorChain<TService, TImplementation>? next,
        ServiceDecoratorDescriptorChainOptions options,
        Type decoratorType)
    {
        _chainFlags = ComposeChainFlags(next);
        _descriptorFlags = ComposeDescriptorFlags(options, decoratorType);
        Next = next;
    }

    public virtual object? ImplementationServiceKey => null;

    public bool UseStandaloneImplementationService => ImplementationServiceKey is not null;
    public bool UsePreRegistrationValidation => !HasFlag(DescriptorFlags.SkipPreRegistrationValidation);

    public bool IsServiceInterface => HasFlag(ChainFlags.IsServiceInterface);
    public bool IsServiceDisposable => HasFlag(ChainFlags.IsServiceDisposable);
    public bool IsServiceAsyncDisposable => HasFlag(ChainFlags.IsServiceAsyncDisposable);

    public bool IsImplementationServiceDisposable => HasFlag(ChainFlags.IsImplementationServiceDisposable);
    public bool IsImplementationServiceAsyncDisposable => HasFlag(ChainFlags.IsImplementationServiceAsyncDisposable);

    public bool IsDecoratorDisposable => HasFlag(DescriptorFlags.IsDecoratorDisposable);
    public bool IsDecoratorAsyncDisposable => HasFlag(DescriptorFlags.IsDecoratorAsyncDisposable);

    public bool IsDisposableContainerAllowed => HasFlag(DescriptorFlags.IsDisposableContainerAllowed);

    private bool HasFlag(ChainFlags flag) => (_chainFlags & flag) == flag;
    private bool HasFlag(DescriptorFlags flag) => (_descriptorFlags & flag) == flag;

    internal abstract TService CreateDecorator(IServiceProvider serviceProvider, TService service);

    private class Node<TDecorator>(
        ServiceDecoratorDescriptorChain<TService, TImplementation>? next,
        ServiceDecoratorDescriptorChainOptions options) :
        ServiceDecoratorDescriptorChain<TService, TImplementation>(next, options, typeof(TDecorator))
        where TDecorator : class, TService
    {
        internal override TService CreateDecorator(IServiceProvider serviceProvider, TService service) =>
            ServiceDecoratorActivator.CreateDecorator<TService, TDecorator>(serviceProvider, service);
    }

    private class ImplementationServiceKeyNode<TDecorator> : Node<TDecorator>
        where TDecorator : class, TService
    {
        public override object ImplementationServiceKey { get; }

        public ImplementationServiceKeyNode(
            ServiceDecoratorDescriptorChain<TService, TImplementation>? next,
            ServiceDecoratorDescriptorChainOptions options,
            object? implementationServiceKey) : base(next, options)
        {
            ImplementationServiceKey = implementationServiceKey is null ? this : implementationServiceKey;
        }
    }

    public static ServiceDecoratorDescriptorChain<TService, TImplementation> CreateNode<TDecorator>(
        ServiceDecoratorDescriptorChain<TService, TImplementation>? next,
        ServiceDecoratorDescriptorChainOptions options = default,
        object? implementationServiceKey = null)
        where TDecorator : class, TService
    {
        var useDefaultImplementationServiceKey = options.HasOption(Options.UseDefaultImplementationServiceKey);
        var useStandaloneImplementationService = implementationServiceKey is not null || useDefaultImplementationServiceKey;

        if (useStandaloneImplementationService)
        {
            if (useDefaultImplementationServiceKey)
                implementationServiceKey = null;

            return new ImplementationServiceKeyNode<TDecorator>(next, options, implementationServiceKey);
        }

        return new Node<TDecorator>(next, options);
    }

    private static ChainFlags ComposeChainFlags(ServiceDecoratorDescriptorChain<TService, TImplementation>? next)
    {
        if (next is not null)
            return next._chainFlags;

        var serviceType = typeof(TService);
        var implementationServiceType = typeof(TImplementation);

        var isServiceInterface = serviceType.IsInterface;
        var isServiceDisposable = serviceType.IsDisposable();
        var isServiceAsyncDisposable = serviceType.IsAsyncDisposable();
        var isImplementationServiceDisposable = implementationServiceType.IsDisposable();
        var isImplementationServiceAsyncDisposable = implementationServiceType.IsAsyncDisposable();

        ChainFlags flags = new();

        if (isServiceInterface) flags |= ChainFlags.IsServiceInterface;
        if (isServiceDisposable) flags |= ChainFlags.IsServiceDisposable;
        if (isServiceAsyncDisposable) flags |= ChainFlags.IsServiceAsyncDisposable;
        if (isImplementationServiceDisposable) flags |= ChainFlags.IsImplementationServiceDisposable;
        if (isImplementationServiceAsyncDisposable) flags |= ChainFlags.IsImplementationServiceAsyncDisposable;

        return flags;
    }

    private static DescriptorFlags ComposeDescriptorFlags(ServiceDecoratorDescriptorChainOptions options, Type decoratorType)
    {
        var isDecoratorDisposable = decoratorType.IsDisposable();
        var isDecoratorAsyncDisposable = decoratorType.IsAsyncDisposable();
        var isDisposableContainerAllowed = options.HasOption(Options.IsDisposableContainerAllowed);
        var skipPreRegistrationValidation = options.HasOption(Options.SkipPreRegistrationValidation);

        DescriptorFlags flags = new();

        if (isDecoratorDisposable) flags |= DescriptorFlags.IsDecoratorDisposable;
        if (isDecoratorAsyncDisposable) flags |= DescriptorFlags.IsDecoratorAsyncDisposable;
        if (isDisposableContainerAllowed) flags |= DescriptorFlags.IsDisposableContainerAllowed;
        if (skipPreRegistrationValidation) flags |= DescriptorFlags.SkipPreRegistrationValidation;

        return flags;
    }

    [Flags]
    private enum ChainFlags : byte
    {
        IsServiceInterface = 0x01,
        IsServiceDisposable = 0x02,
        IsServiceAsyncDisposable = 0x04,
        IsImplementationServiceDisposable = 0x08,
        IsImplementationServiceAsyncDisposable = 0x10
    }

    [Flags]
    private enum DescriptorFlags : byte
    {
        IsDecoratorDisposable = 0x01,
        IsDecoratorAsyncDisposable = 0x02,
        IsDisposableContainerAllowed = 0x04,
        SkipPreRegistrationValidation = 0x08
    }
}
