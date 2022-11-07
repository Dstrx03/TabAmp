using Microsoft.Extensions.DependencyInjection;
using TabAmp.IO;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddIOServices(this IServiceCollection services)
        {
            services.AddTransient<ITabFileReader, TabFileReader>();
            services.AddTabFileReaderContext();
            services.AddScoped<TabFileReadingProcedureFactory>();
            services.AddScoped<GP5ReadingProcedure>();
            services.AddScoped<GP5BasicTypesReader>();
            services.AddScoped<ISequentialStreamReader, SimpleSequentialStreamReader>();

            return services;
        }

        private static IServiceCollection AddTabFileReaderContext(this IServiceCollection services) =>
            TabFileReaderContextFactory.ConfigureServices.AddTabFileReaderContext(services);
    }
}

namespace TabAmp.IO
{
    public partial class TabFileReaderContextFactory
    {
        // TODO: Use file modifier (C# 11)
        internal static class ConfigureServices
        {
            public static IServiceCollection AddTabFileReaderContext(IServiceCollection services)
            {
                services.AddScoped<TabFileReaderContextFactory>();
                services.AddScoped<TabFileReaderContext>();
                services.AddScoped<ITabFileReaderContext>(GetTabFileReaderContextImplementation);

                return services;
            }

            private static ITabFileReaderContext GetTabFileReaderContextImplementation(IServiceProvider serviceProvider)
            {
                var context = serviceProvider.GetRequiredService<TabFileReaderContext>();
                if (context.Signed)
                    return context;
                throw new InvalidOperationException("Cannot inject unsigned context.");
            }
        }
    }
}
