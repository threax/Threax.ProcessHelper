using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Threax.ProcessHelper
{
    public interface IShellRunner<T>
    {
        JToken RunProcess(FormattableString command, string invalidExitCodeMessage = "Invalid exit code for process.", int validExitCode = 0);
        JToken RunProcess(IShellCommandBuilder builder, string invalidExitCodeMessage = "Invalid exit code for process.", int validExitCode = 0);
        TResult? RunProcess<TResult>(FormattableString command, string invalidExitCodeMessage = "Invalid exit code for process.", int validExitCode = 0);
        TResult? RunProcess<TResult>(IShellCommandBuilder builder, string invalidExitCodeMessage = "Invalid exit code for process.", int validExitCode = 0);
        JToken RunProcess(IEnumerable<FormattableString> command, string invalidExitCodeMessage = "Invalid exit code for process.", int validExitCode = 0);
        TResult? RunProcess<TResult>(IEnumerable<FormattableString> command, string invalidExitCodeMessage = "Invalid exit code for process.", int validExitCode = 0);
        void RunProcessVoid(IEnumerable<FormattableString> command, string invalidExitCodeMessage = "Invalid exit code for process.", int validExitCode = 0);
        void RunProcessVoid(FormattableString command, string invalidExitCodeMessage = "Invalid exit code for process.", int validExitCode = 0);
        void RunProcessVoid(IShellCommandBuilder builder, string invalidExitCodeMessage = "Invalid exit code for process.", int validExitCode = 0);

        Task<JToken> RunProcessAsync(FormattableString command, string invalidExitCodeMessage = "Invalid exit code for process.", int validExitCode = 0);
        Task<JToken> RunProcessAsync(IShellCommandBuilder builder, string invalidExitCodeMessage = "Invalid exit code for process.", int validExitCode = 0);
        Task<TResult?> RunProcessAsync<TResult>(FormattableString command, string invalidExitCodeMessage = "Invalid exit code for process.", int validExitCode = 0);
        Task<TResult?> RunProcessAsync<TResult>(IShellCommandBuilder builder, string invalidExitCodeMessage = "Invalid exit code for process.", int validExitCode = 0);
        Task<JToken> RunProcessAsync(IEnumerable<FormattableString> command, string invalidExitCodeMessage = "Invalid exit code for process.", int validExitCode = 0);
        Task<TResult?> RunProcessAsync<TResult>(IEnumerable<FormattableString> command, string invalidExitCodeMessage = "Invalid exit code for process.", int validExitCode = 0);
        Task RunProcessVoidAsync(IEnumerable<FormattableString> command, string invalidExitCodeMessage = "Invalid exit code for process.", int validExitCode = 0);
        Task RunProcessVoidAsync(FormattableString command, string invalidExitCodeMessage = "Invalid exit code for process.", int validExitCode = 0);
        Task RunProcessVoidAsync(IShellCommandBuilder builder, string invalidExitCodeMessage = "Invalid exit code for process.", int validExitCode = 0);
        IShellCommandBuilder CreateCommandBuilder();
        int RunProcessGetExit(IEnumerable<FormattableString> command);
        int RunProcessGetExit(FormattableString command);
        Task<int> RunProcessGetExitAsync(IEnumerable<FormattableString> command);
        Task<int> RunProcessGetExitAsync(FormattableString command);
    }

    public interface IShellRunner : IShellRunner<IShellRunner>
    {

    }
}