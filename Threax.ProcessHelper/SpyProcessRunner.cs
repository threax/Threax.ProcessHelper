using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Threax.ProcessHelper
{
    public class SpyProcessRunner : IProcessRunner
    {
        private readonly IProcessRunner child;
        private ProcessEvents spyEvents = new ProcessEvents();

        public SpyProcessRunner(IProcessRunner child)
        {
            this.child = child;
        }

        public ProcessEvents Events
        {
            get => spyEvents;
            set => spyEvents = value;
        }

        public int Run(ProcessStartInfo startInfo, ProcessEvents? events = null)
        {
            return child.Run(startInfo, new ProcessEvents()
            {
                ProcessCreated = p =>
                {
                    spyEvents.ProcessCreated?.Invoke(p);
                    events?.ProcessCreated?.Invoke(p);
                },
                ProcessCompleted = p =>
                {
                    spyEvents.ProcessCompleted?.Invoke(p);
                    events?.ProcessCompleted?.Invoke(p);
                },
                ErrorDataReceived = (s, e) =>
                {
                    spyEvents?.ErrorDataReceived?.Invoke(s, e);
                    events?.ErrorDataReceived?.Invoke(s, e);
                },
                OutputDataReceived = (s, e) =>
                {
                    spyEvents?.OutputDataReceived?.Invoke(s, e);
                    events?.OutputDataReceived?.Invoke(s, e);
                }
            });
        }
    }
}
