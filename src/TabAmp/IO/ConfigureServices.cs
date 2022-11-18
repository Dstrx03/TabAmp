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
            services.AddTransient<IPathParser, PathParser>();

            return services;
        }

        private static IServiceCollection AddTabFileReaderContext(this IServiceCollection services) =>
            TabFileReaderContextBuilder.ConfigureServices.AddTabFileReaderContext(services);
    }
}

namespace TabAmp.IO
{
    public partial class TabFileReaderContextBuilder
    {
        internal static class ConfigureServices
        {
            public static IServiceCollection AddTabFileReaderContext(IServiceCollection services)
            {
                services.AddScoped(x => x.CreateInstanceForNonRootScope<TabFileReaderContextBuilder>());
                services.AddScoped(x => x.CreateInstanceForNonRootScope<TabFileReaderContext>());
                services.AddScoped(GetTabFileReaderContextImplementation);

                return services;
            }

            private static ITabFileReaderContext GetTabFileReaderContextImplementation(IServiceProvider serviceProvider)
            {
                var context = serviceProvider.GetRequiredService<TabFileReaderContext>();
                if (!context.IsBuilt)
                    throw new InvalidOperationException($"Cannot resolve not built '{typeof(TabFileReaderContext)}'.");
                return context;
            }
        }
    }
}
