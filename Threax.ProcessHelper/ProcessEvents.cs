using System;
using System.Diagnostics;

namespace Threax.ProcessHelper;

public class ProcessEvents
{
    public Action<Process>? ProcessCreated { get; set; }
    public Action<Process>? ProcessCompleted { get; set; }
    public Action<Object, ProcessEventArgs>? OutputDataReceived { get; set; }
    public Action<Object, ProcessEventArgs>? ErrorDataReceived { get; set; }
}
