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
    }
}
