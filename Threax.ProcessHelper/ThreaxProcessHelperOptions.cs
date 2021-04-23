using System;
using System.Collections.Generic;
using System.Text;

namespace Threax.ProcessHelper
{
    public class ThreaxProcessHelperOptions<T>
    {
        public Func<IProcessRunner, IServiceProvider, IProcessRunner>? SetupRunner { get; set; }
    }
}
