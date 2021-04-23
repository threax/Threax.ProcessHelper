using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Threax.ProcessHelper;

namespace Microsoft.Extensions.DependencyInjection
{
    class DefaultInstanceType { }

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddThreaxProcessHelper<T>(this IServiceCollection services, Action<ThreaxProcessHelperOptions<T>>? configure = null)
        {
            var options = new ThreaxProcessHelperOptions<T>();
            configure?.Invoke(options);

            IProcessRunnerFactory<T> factory = options.SetupRunner != null 
                ? new CustomProcessRunnerFactory<T>(() => options.SetupRunner(new ProcessRunner())) 
                : new ProcessRunnerFactory<T>();

            services.AddSingleton<IProcessRunnerFactory<T>>(factory);
            services.AddSingleton<IObjectPropertyFinder<T>, ObjectPropertyFinder<T>>();

            return services;
        }
    }
}
