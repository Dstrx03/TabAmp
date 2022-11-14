namespace TabAmp.IO;

internal static class ServiceProviderExtensions
{
    private const string PropertyIsRootScope = "IsRootScope";

    public static bool IsRootScope(this IServiceProvider serviceProvider) =>
        (bool)serviceProvider.GetType().GetProperty(PropertyIsRootScope).GetValue(serviceProvider);
}
