using Microsoft.Extensions.DependencyInjection;
using TabAmp.IO;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddIOServices(this IServiceCollection services)
        {
            services.AddTabFileReader();
            services.AddTransient<GP5ReadingProcedure>();

            return services;
        }

        private static IServiceCollection AddTabFileReader(this IServiceCollection services) =>
            TabFileReader.ConfigureServices.AddTabFileReader(services);
    }
}

namespace TabAmp.IO
{
    public partial class TabFileReader
    {
        internal static class ConfigureServices
        {
            public static IServiceCollection AddTabFileReader(IServiceCollection services)
            {
                services.AddTransient<ITabFileReader, TabFileReader>();
                services.AddScoped<TabFileReaderContext>();
                services.AddScoped<ITabFileReaderContext>(x => x.GetRequiredService<TabFileReaderContext>());

                return services;
            }
        }
    }
}
