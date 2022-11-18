using Microsoft.Extensions.DependencyInjection;

namespace TabAmp.IO;

internal static class ServiceProviderExtensions
{
    private const string PropertyIsRootScope = "IsRootScope";

    public static bool IsRootScope(this IServiceProvider serviceProvider) =>
        (bool)serviceProvider.GetType().GetProperty(PropertyIsRootScope).GetValue(serviceProvider);

    public static T CreateInstanceForNonRootScope<T>(this IServiceProvider serviceProvider)
    {
        if (serviceProvider.IsRootScope())
            throw new InvalidOperationException($"Cannot resolve '{typeof(T)}' from root provider.");
        return serviceProvider.CreateInstance<T>();
    }

    private static T CreateInstance<T>(this IServiceProvider serviceProvider)
    {
        var type = typeof(T);
        var constructor = type.GetConstructors()[0];
        var args = new List<object>();
        foreach (var p in constructor.GetParameters())
            args.Add(serviceProvider.GetRequiredService(p.ParameterType));
        var instance = Activator.CreateInstance(type, args.ToArray());
        return (T)instance;
    }
}
