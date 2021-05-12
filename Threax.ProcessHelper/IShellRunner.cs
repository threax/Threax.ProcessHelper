using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Threax.ProcessHelper
{
    public interface IShellRunner<T>
    {
        JToken RunProcess(FormattableString command, int validExitCode = 0, string invalidExitCodeMessage = "Invalid exit code for process.");
        JToken RunProcess(IShellCommandBuilder builder, int validExitCode = 0, string invalidExitCodeMessage = "Invalid exit code for process.");
        TResult? RunProcess<TResult>(FormattableString command, int validExitCode = 0, string invalidExitCodeMessage = "Invalid exit code for process.");
        TResult? RunProcess<TResult>(IShellCommandBuilder builder, int validExitCode = 0, string invalidExitCodeMessage = "Invalid exit code for process.");
        JToken RunProcess(IEnumerable<FormattableString> command, int validExitCode = 0, string invalidExitCodeMessage = "Invalid exit code for process.");
        TResult? RunProcess<TResult>(IEnumerable<FormattableString> command, int validExitCode = 0, string invalidExitCodeMessage = "Invalid exit code for process.");
        void RunProcessVoid(IEnumerable<FormattableString> command, int validExitCode = 0, string invalidExitCodeMessage = "Invalid exit code for process.");
        void RunProcessVoid(FormattableString command, int validExitCode = 0, string invalidExitCodeMessage = "Invalid exit code for process.");
        void RunProcessVoid(IShellCommandBuilder builder, int validExitCode = 0, string invalidExitCodeMessage = "Invalid exit code for process.");

        Task<JToken> RunProcessAsync(FormattableString command, int validExitCode = 0, string invalidExitCodeMessage = "Invalid exit code for process.");
        Task<JToken> RunProcessAsync(IShellCommandBuilder builder, int validExitCode = 0, string invalidExitCodeMessage = "Invalid exit code for process.");
        Task<TResult?> RunProcessAsync<TResult>(FormattableString command, int validExitCode = 0, string invalidExitCodeMessage = "Invalid exit code for process.");
        Task<TResult?> RunProcessAsync<TResult>(IShellCommandBuilder builder, int validExitCode = 0, string invalidExitCodeMessage = "Invalid exit code for process.");
        Task<JToken> RunProcessAsync(IEnumerable<FormattableString> command, int validExitCode = 0, string invalidExitCodeMessage = "Invalid exit code for process.");
        Task<TResult?> RunProcessAsync<TResult>(IEnumerable<FormattableString> command, int validExitCode = 0, string invalidExitCodeMessage = "Invalid exit code for process.");
        Task RunProcessVoidAsync(IEnumerable<FormattableString> command, int validExitCode = 0, string invalidExitCodeMessage = "Invalid exit code for process.");
        Task RunProcessVoidAsync(FormattableString command, int validExitCode = 0, string invalidExitCodeMessage = "Invalid exit code for process.");
        Task RunProcessVoidAsync(IShellCommandBuilder builder, int validExitCode = 0, string invalidExitCodeMessage = "Invalid exit code for process.");
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