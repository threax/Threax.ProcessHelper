using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;

namespace Threax.ProcessHelper
{
    public class LoggingProcessRunner<TLog> : IProcessRunner<TLog>
    {
        private readonly IProcessRunner child;
        private readonly ILogger<TLog> logger;

        public LoggingProcessRunner(IProcessRunner child, ILogger<TLog> logger)
        {
            this.child = child;
            this.logger = logger;
        }

        public int Run(ProcessStartInfo startInfo, ProcessEvents? events = null)
        {
            return child.Run(startInfo, new ProcessEvents()
            {
                ProcessCreated = events?.ProcessCreated,
                ProcessCompleted = events?.ProcessCompleted,
                ErrorDataReceived = (s, e) =>
                {
                    events?.ErrorDataReceived?.Invoke(s, e);

                    if (e.AllowOutput && !String.IsNullOrEmpty(e.DataReceivedEventArgs.Data))
                    {
                        logger.LogWarning(e.DataReceivedEventArgs.Data);
                    }
                },
                OutputDataReceived = (s, e) =>
                {
                    events?.OutputDataReceived?.Invoke(s, e);

                    if (e.AllowOutput && !String.IsNullOrEmpty(e.DataReceivedEventArgs.Data))
                    {
                        logger.LogInformation(e.DataReceivedEventArgs.Data);
                    }
                }
            });
        }
    }
}
