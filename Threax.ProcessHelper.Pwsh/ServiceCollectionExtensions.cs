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

            services.AddScoped<IPowershellCoreRunner, PowershellCoreRunner>();
            services.TryAddScoped<IShellRunner>(s => s.GetRequiredService<IPowershellCoreRunner>());

            return services;
        }
    }
}
