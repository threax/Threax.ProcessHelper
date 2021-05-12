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
        public static IServiceCollection AddThreaxPwshShellRunner(this IServiceCollection services, Action<ThreaxPwshProcessHelperOptions>? configure = null)
        {
            var options = new ThreaxPwshProcessHelperOptions();
            configure?.Invoke(options);

            services.TryAddScoped<IProcessRunnerFactory>(
                s => new CustomProcessRunnerFactory(() => CreateRunner(s, options)));

            services.TryAddScoped<IShellRunner, PowershellCoreRunner>();

            return services;
        }

        public static IServiceCollection AddThreaxPwshShellRunner<T>(this IServiceCollection services, Action<ThreaxPwshProcessHelperOptions<T>>? configure = null)
        {
            var options = new ThreaxPwshProcessHelperOptions<T>();
            configure?.Invoke(options);

            services.TryAddScoped<IProcessRunnerFactory<T>>(
                s => new CustomProcessRunnerFactory<T>(() => CreateRunner(s, options)));

            services.TryAddScoped<IShellRunner<T>, PowershellCoreRunner<T>>();

            return services;
        }

        private static IProcessRunner CreateRunner<T>(IServiceProvider s, ThreaxPwshProcessHelperOptions<T> options)
        {
            IProcessRunner runner = new ProcessRunner();
            if (options.IncludeLogOutput)
            {
                try
                {
                    var logger = s.GetRequiredService<ILogger<DefaultPwshLog>>();
                    runner = new LoggingProcessRunner<DefaultPwshLog>(runner, logger);
                }
                catch (ObjectDisposedException)
                {
                    //Sometimes this is called after the context is disposed.
                    //If that happens it is ok, logging will not be included.
                }
            }
            if (options.DecorateProcessRunner != null)
            {
                runner = options.DecorateProcessRunner.Invoke(runner);
            }
            return runner;
        }
    }
}
