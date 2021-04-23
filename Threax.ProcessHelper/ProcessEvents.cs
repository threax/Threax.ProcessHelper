using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Threax.ProcessHelper
{
    public class ProcessEvents
    {
        public Action<Process>? ProcessCreated { get; set; }
        public Action<Object, DataReceivedEventArgs>? OutputDataReceived { get; set; }
        public Action<Object, DataReceivedEventArgs>? ErrorDataReceived { get; set; }
    }
}
