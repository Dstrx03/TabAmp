namespace Microsoft.Extensions.DependencyInjection.Decorator;

public static class ServiceCollectionDecoratorExtensions
{
    public static IServiceCollection Todo(this IServiceCollection serviceCollection)
    {
        return serviceCollection;
    }
}
