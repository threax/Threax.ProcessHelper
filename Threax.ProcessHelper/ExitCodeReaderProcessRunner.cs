using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Threax.ProcessHelper
{
    /// <summary>
    /// Read the final line of output from a process as its exit code. This will likley require some
    /// work to ensure the output is correct.
    /// </summary>
    public class ExitCodeReaderProcessRunner : IProcessRunner
    {
        private readonly IProcessRunner child;

        private string? lastLine;

        public ExitCodeReaderProcessRunner(IProcessRunner child)
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
                    events?.ErrorDataReceived?.Invoke(s, e);
                },
                OutputDataReceived = (s, e) =>
                {
                    if (!String.IsNullOrEmpty(e.Data))
                    {
                        lastLine = e.Data;
                    }

                    events?.OutputDataReceived?.Invoke(s, e);
                }
            });
        }

        /// <summary>
        /// The last exit code that was output by the running process. This will only be
        /// valid if the exit code is printed as the last line of output.
        /// </summary>
        public int LastExitCode => lastLine != null ? int.Parse(lastLine) : throw new InvalidOperationException("The process provided no output to read an exit code from.");
    }
}
