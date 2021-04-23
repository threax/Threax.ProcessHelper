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
            startInfo.RedirectStandardError = true;
            startInfo.RedirectStandardOutput = true;
            using (var process = Process.Start(startInfo))
            {
                if (events != null)
                {
                    process.ErrorDataReceived += (s, e) =>
                    {
                        events.ErrorDataReceived?.Invoke(s, e);
                    };
                    process.OutputDataReceived += (s, e) =>
                    {
                        events.OutputDataReceived?.Invoke(s, e);
                    };
                }
                process.BeginErrorReadLine();
                process.BeginOutputReadLine();

                events?.ProcessCreated?.Invoke(process);

                process.WaitForExit();

                return process.ExitCode;
            }
        }
    }
}
