using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Threax.ProcessHelper.Pwsh
{
    public class ThreaxPwshProcessHelperOptions<T>
    {
        /// <summary>
        /// Set this to true to have the log output written during commands.
        /// </summary>
        public bool IncludeLogOutput { get; set; }

        /// <summary>
        /// Create a customized argument builder to use for this powershell runner. If this
        /// is null the default builder will be used.
        /// </summary>
        public Func<IServiceProvider, IPwshArgumentBuilder> CreateArgumentBuilder { get; set; } = s => s.GetRequiredService<IPwshArgumentBuilder>();
    }
}
