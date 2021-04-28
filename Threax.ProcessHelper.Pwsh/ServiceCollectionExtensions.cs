using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using System;
using Threax.ProcessHelper;
using Threax.ProcessHelper.Pwsh;

namespace Microsoft.Extensions.DependencyInjection
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

            services.TryAddScoped<IShellRunner<T>, PowershellCoreRunner<T>>();
            services.TryAddTransient<IShellCommandBuilder, PwshCommandBuilder>();

            return services;
        }
    }
}
