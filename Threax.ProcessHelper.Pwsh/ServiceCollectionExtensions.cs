using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using System;

namespace Threax.ProcessHelper.Pwsh
{
    public static class ServiceCollectionExtensions
    {
        class DefaultInstance { }

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

            services.TryAddScoped<IPowershellCoreRunner<T>>(s =>
            {
                var processFactory = s.GetRequiredService<IProcessRunnerFactory<PowershellCoreRunner<T>>>();
                var objectPropertyFinder = s.GetRequiredService<IObjectPropertyFinder>();
                var argProvider = options.CreateArgumentBuilder(s);
                return new PowershellCoreRunner<T>(processFactory, objectPropertyFinder, argProvider);
            });

            services.TryAddScoped(typeof(IPwshArgumentBuilder<>), typeof(PwshArgumentBuilder<>));
            services.TryAddScoped<IPwshArgumentBuilder>(s => s.GetService<IPwshArgumentBuilder<DefaultInstance>>());

            services.TryAddTransient<IPwshCommandBuilder, PwshCommandBuilder>();

            return services;
        }
    }
}
