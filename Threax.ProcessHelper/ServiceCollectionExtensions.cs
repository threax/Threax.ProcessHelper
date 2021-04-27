using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Threax.ProcessHelper;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddThreaxProcessHelper<T>(this IServiceCollection services, Action<ThreaxProcessHelperOptions<T>>? configure = null)
        {
            var options = new ThreaxProcessHelperOptions<T>();
            configure?.Invoke(options);

            services.TryAddSingleton<IProcessRunnerFactory<T>>(s =>
                options.SetupRunner != null
                ? new CustomProcessRunnerFactory<T>(() => options.SetupRunner(new ProcessRunner(), s))
                : new ProcessRunnerFactory<T>()
            );

            return services;
        }
    }
}
