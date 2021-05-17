using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using System;
using Threax.ProcessHelper;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddThreaxProcessHelper(this IServiceCollection services, Action<ThreaxProcessHelperOptions>? configure = null)
        {
            var options = new ThreaxProcessHelperOptions();
            configure?.Invoke(options);

            services.TryAddScoped<IProcessRunner>(
                s => CreateRunner(s, options));

            return services;
        }

        private static IProcessRunner CreateRunner(IServiceProvider s, ThreaxProcessHelperOptions options)
        {
            IProcessRunner runner = new ProcessRunner();
            if (options.IncludeLogOutput)
            {
                try
                {
                    var logger = s.GetRequiredService<ILogger<DefaultLog>>();
                    runner = new LoggingProcessRunner<DefaultLog>(runner, logger);
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
