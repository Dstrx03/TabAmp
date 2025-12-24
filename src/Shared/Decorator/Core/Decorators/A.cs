using System;
using System.Collections.Generic;
using System.Reflection;
using TabAmp.Shared.Decorator.Core.DescriptorChain;

namespace TabAmp.Shared.Decorator.Core.Decorators;

internal class A<TService> : DispatchProxy, IDisposable
    where TService : notnull
{
    private readonly List<IDisposable> _dspsbls = [];
    private TService? _service;
    protected override object? Invoke(MethodInfo? targetMethod, object?[]? args) => targetMethod.Invoke(_service, args);
    public void Dispose() => throw new NotImplementedException();
    internal void TODO1(TService service, ServiceDecoratorDescriptorChain<TService> descriptor)
    {
        if (descriptor.IsDecoratorDisposable) _dspsbls.Add((IDisposable)service);
    }
    internal TService TODO2(TService service)
    {
        _service = service;
        return (TService)(object)this;
    }
}
