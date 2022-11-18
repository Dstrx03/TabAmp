using Microsoft.Extensions.DependencyInjection;
using TabAmp.IO;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddIOServices(this IServiceCollection services)
        {
            services.AddTransient<ITabFileReader, TabFileReader>();
            services.AddScoped(x => x.CreateInstanceForInnerScope<TabFileReaderContextBuilder>());
            services.AddTabFileReaderContext();
            services.AddScoped(x => x.GetRequiredService<TabFileReaderContextBuilder>().GetContext());
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
            public static IServiceCollection AddTabFileReaderContext(IServiceCollection services) =>
                services.AddScoped(x => x.CreateInstanceForInnerScope<TabFileReaderContext>());
        }
    }
}
