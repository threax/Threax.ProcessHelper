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
        class DefaultPwshLog { }

        public static IServiceCollection AddThreaxPwshShellRunner(this IServiceCollection services, Action<ThreaxPwshProcessHelperOptions>? configure = null)
        {
            var options = new ThreaxPwshProcessHelperOptions();
            configure?.Invoke(options);

            services.TryAddScoped<IProcessRunnerFactory>(s => new CustomProcessRunnerFactory(() =>
            {
                IProcessRunner runner = new ProcessRunner();
                if (options.IncludeLogOutput)
                {
                    var logger = s.GetRequiredService<ILogger<DefaultPwshLog>>();
                    runner = new LoggingProcessRunner<DefaultPwshLog>(runner, logger);
                }
                if (options.DecorateProcessRunner != null)
                {
                    runner = options.DecorateProcessRunner.Invoke(runner);
                }
                return runner;
            }));

            services.TryAddScoped<IShellRunner, PowershellCoreRunner>();
            services.TryAddTransient<IShellCommandBuilderFactory, PwshCommandBuilderFactory>();

            return services;
        }

        public static IServiceCollection AddThreaxPwshShellRunner<T>(this IServiceCollection services, Action<ThreaxPwshProcessHelperOptions<T>>? configure = null)
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
            services.TryAddTransient<IShellCommandBuilderFactory<T>, PwshCommandBuilderFactory<T>>();

            return services;
        }
    }
}
