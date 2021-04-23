using Microsoft.Extensions.DependencyInjection;
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
                    o.SetupRunner = r => new ConsoleOutputProcessRunner(r);
                }
            });

            services.AddScoped<IPowershellCoreRunner<T>, PowershellCoreRunner<T>>();

            return services;
        }
    }
}
