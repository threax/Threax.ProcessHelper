using Newtonsoft.Json.Linq;
using System;

namespace Threax.ProcessHelper.Pwsh
{
    public interface IPowershellCoreRunner<T>
    {
        JToken RunProcess(FormattableString command, int validExitCode = 0, string invalidExitCodeMessage = "Invalid exit code for process.");
        TResult? RunProcess<TResult>(FormattableString command, int validExitCode = 0, string invalidExitCodeMessage = "Invalid exit code for process.");
        int RunProcessVoid(FormattableString command);
    }
}