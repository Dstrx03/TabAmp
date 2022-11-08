﻿using Microsoft.Extensions.DependencyInjection;
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
            TabFileReaderContextBuilder.ConfigureServices.AddTabFileReaderContext(services);
    }
}

namespace TabAmp.IO
{
    public partial class TabFileReaderContextBuilder
    {
        // TODO: Use file modifier (C# 11)
        internal static class ConfigureServices
        {
            public static IServiceCollection AddTabFileReaderContext(IServiceCollection services)
            {
                services.AddScoped<TabFileReaderContextBuilder>();
                services.AddScoped<TabFileReaderContext>();
                services.AddScoped<ITabFileReaderContext>(GetTabFileReaderContextImplementation);

                return services;
            }

            private static ITabFileReaderContext GetTabFileReaderContextImplementation(IServiceProvider serviceProvider)
            {
                if (IsRootScope(serviceProvider))
                    throw new InvalidOperationException($"Cannot resolve '{typeof(TabFileReaderContext)}' from root provider.");
                var context = serviceProvider.GetRequiredService<TabFileReaderContext>();
                if (!context.IsSigned)
                    throw new InvalidOperationException($"Unsigned '{typeof(TabFileReaderContext)}' cannot be resolved.");
                return context;
            }

            private static bool IsRootScope(IServiceProvider serviceProvider) =>
                (bool)serviceProvider.GetType().GetProperty("IsRootScope").GetValue(serviceProvider);
        }
    }
}
