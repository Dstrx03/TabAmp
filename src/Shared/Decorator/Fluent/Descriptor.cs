using System;
using Microsoft.Extensions.DependencyInjection.Decorator;

namespace TabAmp.Shared.Decorator.Fluent;

internal interface IDescriptor<TService>
{
    TService DecorateService(TService service, IServiceProvider serviceProvider);
}

internal record Descriptor<TService, TDecorator> : IDescriptor<TService>// TODO: name
    where TService : notnull
    where TDecorator: notnull, TService
{
    public TService DecorateService(TService service, IServiceProvider serviceProvider) =>
        serviceProvider.DecorateService<TService, TDecorator>(service);
}
