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
            services.AddTransient<GP5BasicTypesReader>();
            services.AddScoped<ISequentialStreamReader, SimpleSequentialStreamReader>();

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
        // TODO: Use file modifier (C# 11)
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
