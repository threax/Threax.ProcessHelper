using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace Threax.ProcessHelper
{
    public interface IShellRunner<T>
    {
        JToken RunProcess(FormattableString command, int validExitCode = 0, string invalidExitCodeMessage = "Invalid exit code for process.");
        JToken RunProcess(IShellCommandBuilder<T> builder, int validExitCode = 0, string invalidExitCodeMessage = "Invalid exit code for process.");
        TResult? RunProcess<TResult>(FormattableString command, int validExitCode = 0, string invalidExitCodeMessage = "Invalid exit code for process.");
        TResult? RunProcess<TResult>(IShellCommandBuilder<T> builder, int validExitCode = 0, string invalidExitCodeMessage = "Invalid exit code for process.");
        JToken RunProcess(IEnumerable<FormattableString> command, int validExitCode = 0, string invalidExitCodeMessage = "Invalid exit code for process.");
        TResult? RunProcess<TResult>(IEnumerable<FormattableString> command, int validExitCode = 0, string invalidExitCodeMessage = "Invalid exit code for process.");
        void RunProcessVoid(IEnumerable<FormattableString> command, int validExitCode = 0, string invalidExitCodeMessage = "Invalid exit code for process.");
        void RunProcessVoid(FormattableString command, int validExitCode = 0, string invalidExitCodeMessage = "Invalid exit code for process.");
        void RunProcessVoid(IShellCommandBuilder<T> builder, int validExitCode = 0, string invalidExitCodeMessage = "Invalid exit code for process.");
    }

    public interface IShellRunner : IShellRunner<IShellRunner>
    {

    }
}