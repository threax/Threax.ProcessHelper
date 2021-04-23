using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Threax.ProcessHelper
{
    public class ConsoleOutputProcessRunner : IProcessRunner
    {
        private readonly IProcessRunner child;

        public ConsoleOutputProcessRunner(IProcessRunner child)
        {
            this.child = child;
        }

        public int Run(ProcessStartInfo startInfo, ProcessEvents? events = null)
        {
            return child.Run(startInfo, new ProcessEvents()
            {
                ProcessCreated = events?.ProcessCreated,
                ErrorDataReceived = (s, e) =>
                {
                    if (!String.IsNullOrEmpty(e.Data))
                    {
                        Console.Error.WriteLine(e.Data);
                    }

                    events?.ErrorDataReceived?.Invoke(s, e);
                },
                OutputDataReceived = (s, e) =>
                {
                    if (!String.IsNullOrEmpty(e.Data))
                    {
                        Console.WriteLine(e.Data);
                    }

                    events?.OutputDataReceived?.Invoke(s, e);
                }
            });
        }
    }
}
