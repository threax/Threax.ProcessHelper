using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace Threax.ProcessHelper.Pwsh
{
    public interface IPowershellCoreRunner<T>
    {
        JToken RunProcess(FormattableString command, int validExitCode = 0, string invalidExitCodeMessage = "Invalid exit code for process.");
        JToken RunProcess(IPwshCommandBuilder builder, int validExitCode = 0, string invalidExitCodeMessage = "Invalid exit code for process.");
        TResult? RunProcess<TResult>(FormattableString command, int validExitCode = 0, string invalidExitCodeMessage = "Invalid exit code for process.");
        TResult? RunProcess<TResult>(IPwshCommandBuilder builder, int validExitCode = 0, string invalidExitCodeMessage = "Invalid exit code for process.");
        void RunProcessVoid(IEnumerable<FormattableString> command, int validExitCode = 0, string invalidExitCodeMessage = "Invalid exit code for process.");
        void RunProcessVoid(FormattableString command, int validExitCode = 0, string invalidExitCodeMessage = "Invalid exit code for process.");
        void RunProcessVoid(IPwshCommandBuilder builder, int validExitCode = 0, string invalidExitCodeMessage = "Invalid exit code for process.");
    }
}