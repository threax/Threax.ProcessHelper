using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using Threax.ProcessHelper;

namespace Threax.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddThreaxProcessHelper(this IServiceCollection services, Action<ThreaxProcessHelperOptions>? configure = null)
    {
        var options = new ThreaxProcessHelperOptions();
        configure?.Invoke(options);

        services.TryAddScoped<IProcessRunner>(s =>
        {
            IProcessRunner runner = new ProcessRunner();
            if (options.DecorateProcessRunner != null)
            {
                runner = options.DecorateProcessRunner.Invoke(s, runner);
            }
            return runner;
        });

        return services;
    }
}
