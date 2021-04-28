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

            if (options.IncludeLogOutput)
            {
                services.TryAddScoped<IProcessRunnerFactory<T>>(s => new CustomProcessRunnerFactory<T>(() =>
                {
                    var logger = s.GetRequiredService<ILogger<T>>();
                    var runner = new LoggingProcessRunner<T>(new ProcessRunner(), logger);
                    return runner;
                }));
            }
            else
            {
                services.TryAddScoped<IProcessRunnerFactory<T>, ProcessRunnerFactory<T>>();
            }

            services.TryAddScoped<IShellRunner<T>, PowershellCoreRunner<T>>();
            services.TryAddTransient<IShellCommandBuilder, PwshCommandBuilder>();

            return services;
        }
    }
}
