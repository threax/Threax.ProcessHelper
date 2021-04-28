using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Threax.ProcessHelper.Pwsh
{
    public class ThreaxPwshProcessHelperOptions<T>
    {
        /// <summary>
        /// Set this to true to have the log output written during commands. Default: true
        /// </summary>
        public bool IncludeLogOutput { get; set; } = true;
    }

    public class ThreaxPwshProcessHelperOptions : ThreaxPwshProcessHelperOptions<ThreaxPwshProcessHelperOptions>
    {

    }
}
