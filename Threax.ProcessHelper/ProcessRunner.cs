using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Threax.ProcessHelper
{
    public class ProcessRunner : IProcessRunner
    {
        public int Run(ProcessStartInfo startInfo, ProcessEvents? events = null)
        {
            var errorEventArgs = new ProcessEventArgs();
            errorEventArgs.Reset();
            var outputEventArgs = new ProcessEventArgs();
            outputEventArgs.Reset();

            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardError = true;
            startInfo.RedirectStandardOutput = true;
            using var process = Process.Start(startInfo);
            if (events != null)
            {
                process.ErrorDataReceived += (s, e) =>
                {
                    errorEventArgs.DataReceivedEventArgs = e;
                    events.ErrorDataReceived?.Invoke(s, errorEventArgs);
                    errorEventArgs.Reset();
                };
                process.OutputDataReceived += (s, e) =>
                {
                    outputEventArgs.DataReceivedEventArgs = e;
                    events.OutputDataReceived?.Invoke(s, outputEventArgs);
                    outputEventArgs.Reset();
                };
            }
            process.BeginErrorReadLine();
            process.BeginOutputReadLine();

            events?.ProcessCreated?.Invoke(process);

            process.WaitForExit();

            events?.ProcessCompleted?.Invoke(process);

            return process.ExitCode;
        }
    }
}
