﻿using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Threax.ProcessHelper
{
    public class LoggingProcessRunner<TLog> : IProcessRunner
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
                    if (!String.IsNullOrEmpty(e.Data))
                    {
                        logger.LogWarning(e.Data);
                    }

                    events?.ErrorDataReceived?.Invoke(s, e);
                },
                OutputDataReceived = (s, e) =>
                {
                    if (!String.IsNullOrEmpty(e.Data))
                    {
                        logger.LogInformation(e.Data);
                    }

                    events?.OutputDataReceived?.Invoke(s, e);
                }
            });
        }
    }
}
