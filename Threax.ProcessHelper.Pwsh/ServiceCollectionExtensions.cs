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

            services.TryAddScoped<IProcessRunner>(
                s => CreateRunner(s, options));

            services.AddScoped<IPowershellCoreRunner, PowershellCoreRunner>();
            services.TryAddScoped<IShellRunner>(s => s.GetRequiredService<IPowershellCoreRunner>());

            return services;
        }

        private static IProcessRunner CreateRunner(IServiceProvider s, ThreaxPwshProcessHelperOptions options)
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
