using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using System;

namespace Threax.ProcessHelper.Pwsh
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddThreaxPwshProcessHelper<T>(this IServiceCollection services, Action<ThreaxPwshProcessHelperOptions<T>>? configure = null)
        {
            var options = new ThreaxPwshProcessHelperOptions<T>();
            configure?.Invoke(options);

            services.AddThreaxProcessHelper<PowershellCoreRunner<T>>(o =>
            {
                if (options.IncludeLogOutput)
                {
                    o.SetupRunner = (r, s) => new LoggingProcessRunner<T>(r, s.GetRequiredService<ILogger<LoggingProcessRunner<T>>>());
                }
            });

            services.TryAddScoped<IPowershellCoreRunner<T>, PowershellCoreRunner<T>>();
            services.TryAddTransient<IPwshCommandBuilder, PwshCommandBuilder>();

            return services;
        }
    }
}
