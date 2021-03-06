using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Threax.ProcessHelper
{
    public class ProcessEvents
    {
        public Action<Process>? ProcessCreated { get; set; }
        public Action<Process>? ProcessCompleted { get; set; }
        public Action<Object, ProcessEventArgs>? OutputDataReceived { get; set; }
        public Action<Object, ProcessEventArgs>? ErrorDataReceived { get; set; }
    }
}
