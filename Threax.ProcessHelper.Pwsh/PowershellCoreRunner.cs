using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Threax.ProcessHelper.Pwsh
{
    public class PowershellCoreRunner<T> : IShellRunner<T>
    {
        private readonly IProcessRunnerFactory<T> processRunnerFactory;

        public PowershellCoreRunner(IProcessRunnerFactory<T> processRunnerFactory)
        {
            this.processRunnerFactory = processRunnerFactory;
        }

        public int RunProcessGetExit(IEnumerable<FormattableString> command)
        {
            var escapedCommand = command.GetPwshEnvString(out var args);
            return DoRunProcessGetExit(escapedCommand, args);
        }

        public int RunProcessGetExit(FormattableString command)
        {
            var escapedCommand = command.GetPwshEnvString(out var args);
            return DoRunProcessGetExit(escapedCommand, args);
        }

        public void RunProcessVoid(IEnumerable<FormattableString> command, string invalidExitCodeMessage = "Invalid exit code for process.", int validExitCode = 0)
        {
            var escapedCommand = command.GetPwshEnvString(out var args);
            DoRunProcessVoid(escapedCommand, args, invalidExitCodeMessage, validExitCode);
        }

        public void RunProcessVoid(FormattableString command, string invalidExitCodeMessage = "Invalid exit code for process.", int validExitCode = 0)
        {
            var escapedCommand = command.GetPwshEnvString(out var args);
            DoRunProcessVoid(escapedCommand, args, invalidExitCodeMessage, validExitCode);
        }

        private void DoRunProcessVoid(String escapedCommand, IEnumerable<KeyValuePair<string, object?>> args, string invalidExitCodeMessage, int validExitCode)
        {
            int exitCode = DoRunProcessGetExit(escapedCommand, args);
            if (exitCode != validExitCode)
            {
                throw new InvalidOperationException($"Invalid exit code '{exitCode}' expected '{validExitCode}'. Message: '{invalidExitCodeMessage}'");
            }
        }

        private int DoRunProcessGetExit(string escapedCommand, IEnumerable<KeyValuePair<string, object?>> args)
        {
            var runner = processRunnerFactory.Create();
            var finalCommand = $"{escapedCommand};exit $LASTEXITCODE";
            var startInfo = SetupArgs(finalCommand, args);
            var exitCode = runner.Run(startInfo);
            return exitCode;
        }

        public TResult? RunProcess<TResult>(IEnumerable<FormattableString> command, string invalidExitCodeMessage = "Invalid exit code for process.", int validExitCode = 0)
        {
            var escapedCommand = command.GetPwshEnvString(out var args);
            var result = DoRunProcess(escapedCommand, args, invalidExitCodeMessage, validExitCode);
            return result.ToObject<TResult>();
        }

        public TResult? RunProcess<TResult>(FormattableString command, string invalidExitCodeMessage = "Invalid exit code for process.", int validExitCode = 0)
        {
            var escapedCommand = command.GetPwshEnvString(out var args);
            var result = DoRunProcess(escapedCommand, args, invalidExitCodeMessage, validExitCode);
            return result.ToObject<TResult>();
        }

        public JToken RunProcess(IEnumerable<FormattableString> command, string invalidExitCodeMessage = "Invalid exit code for process.", int validExitCode = 0)
        {
            var escapedCommand = command.GetPwshEnvString(out var args);
            var result = DoRunProcess(escapedCommand, args, invalidExitCodeMessage, validExitCode);
            return result;
        }

        public JToken RunProcess(FormattableString command, string invalidExitCodeMessage = "Invalid exit code for process.", int validExitCode = 0)
        {
            var escapedCommand = command.GetPwshEnvString(out var args);
            var result = DoRunProcess(escapedCommand, args, invalidExitCodeMessage, validExitCode);
            return result;
        }

        private JToken DoRunProcess(String escapedCommand, IEnumerable<KeyValuePair<string, object?>> args, String invalidExitCodeMessage, int validExitCode)
        {
            var runner = new JsonOutputProcessRunner(processRunnerFactory.Create());
            var jsonStart = EscapePwshSingleQuote(runner.JsonStart);
            var jsonEnd = EscapePwshSingleQuote(runner.JsonEnd);

            var finalCommand = $"'{jsonStart}';{escapedCommand};'{jsonEnd}';exit $LASTEXITCODE";
            var startInfo = SetupArgs(finalCommand, args);
            var exitCode = runner.Run(startInfo);
            if (exitCode != validExitCode)
            {
                throw new InvalidOperationException($"Invalid exit code '{exitCode}' expected '{validExitCode}'. Message: '{invalidExitCodeMessage}'");
            }
            return runner.GetResult();
        }

        public void RunProcessVoid(IShellCommandBuilder builder, string invalidExitCodeMessage = "Invalid exit code for process.", int validExitCode = 0)
        {
            var runner = processRunnerFactory.Create();

            var escapedCommand = builder.CreateFinalEscapedCommand(out var args);
            var startInfo = SetupArgs(escapedCommand, args);
            var exitCode = runner.Run(startInfo);
            if (exitCode != validExitCode)
            {
                throw new InvalidOperationException($"Invalid exit code '{exitCode}' expected '{validExitCode}'. Message: '{invalidExitCodeMessage}'");
            }
        }

        public TResult? RunProcess<TResult>(IShellCommandBuilder builder, string invalidExitCodeMessage = "Invalid exit code for process.", int validExitCode = 0)
        {
            var result = RunProcess(builder, invalidExitCodeMessage, validExitCode);
            return result.ToObject<TResult>();
        }

        public JToken RunProcess(IShellCommandBuilder builder, string invalidExitCodeMessage = "Invalid exit code for process.", int validExitCode = 0)
        {
            var runner = new JsonOutputProcessRunner(processRunnerFactory.Create());
            var jsonStart = EscapePwshSingleQuote(runner.JsonStart);
            var jsonEnd = EscapePwshSingleQuote(runner.JsonEnd);

            var escapedCommand = builder.CreateFinalEscapedCommand(out var args);
            var finalCommand = $"{escapedCommand};'{jsonStart}';${builder.ResultVariableName};'{jsonEnd}'";
            var startInfo = SetupArgs(finalCommand, args);
            var exitCode = runner.Run(startInfo);
            if (exitCode != validExitCode)
            {
                throw new InvalidOperationException($"Invalid exit code '{exitCode}' expected '{validExitCode}'. Message: '{invalidExitCodeMessage}'");
            }
            return runner.GetResult();
        }

        private ProcessStartInfo SetupArgs(String finalCommand, IEnumerable<KeyValuePair<String, Object?>> args)
        {
            var startInfo = new ProcessStartInfo("pwsh", $"-c {finalCommand}");
            foreach (var property in args.Where(i => i.Value != null))
            {
                startInfo.Environment[property.Key] = property.Value?.ToString();
            }
            return startInfo;
        }

        private String EscapePwshSingleQuote(String value)
        {
            value = value.Replace("'", "''");
            return value;
        }

        public Task<int> RunProcessGetExitAsync(IEnumerable<FormattableString> command)
        {
            return RunFuncAsync<int>(() => RunProcessGetExit(command));
        }

        public Task<int> RunProcessGetExitAsync(FormattableString command)
        {
            return RunFuncAsync<int>(() => RunProcessGetExit(command));
        }

        public Task<JToken> RunProcessAsync(FormattableString command, string invalidExitCodeMessage = "Invalid exit code for process.", int validExitCode = 0)
        {
            return RunFuncAsync<JToken>(() => RunProcess(command, invalidExitCodeMessage, validExitCode));
        }

        public Task<JToken> RunProcessAsync(IShellCommandBuilder builder, string invalidExitCodeMessage = "Invalid exit code for process.", int validExitCode = 0)
        {
            return RunFuncAsync<JToken>(() => RunProcess(builder, invalidExitCodeMessage, validExitCode));
        }

        public Task<TResult?> RunProcessAsync<TResult>(FormattableString command, string invalidExitCodeMessage = "Invalid exit code for process.", int validExitCode = 0)
        {
            return RunFuncAsync<TResult?>(() => RunProcess<TResult>(command, invalidExitCodeMessage, validExitCode));
        }

        public Task<TResult?> RunProcessAsync<TResult>(IShellCommandBuilder builder, string invalidExitCodeMessage = "Invalid exit code for process.", int validExitCode = 0)
        {
            return RunFuncAsync<TResult?>(() => RunProcess<TResult>(builder, invalidExitCodeMessage, validExitCode));
        }

        public Task<JToken> RunProcessAsync(IEnumerable<FormattableString> command, string invalidExitCodeMessage = "Invalid exit code for process.", int validExitCode = 0)
        {
            return RunFuncAsync<JToken>(() => RunProcess(command, invalidExitCodeMessage, validExitCode));
        }

        public Task<TResult?> RunProcessAsync<TResult>(IEnumerable<FormattableString> command, string invalidExitCodeMessage = "Invalid exit code for process.", int validExitCode = 0)
        {
            return RunFuncAsync<TResult?>(() => RunProcess<TResult>(command, invalidExitCodeMessage, validExitCode));
        }

        public Task RunProcessVoidAsync(IEnumerable<FormattableString> command, string invalidExitCodeMessage = "Invalid exit code for process.", int validExitCode = 0)
        {
            return RunActionAsync(() => RunProcessVoid(command, invalidExitCodeMessage, validExitCode));
        }

        public Task RunProcessVoidAsync(FormattableString command, string invalidExitCodeMessage = "Invalid exit code for process.", int validExitCode = 0)
        {
            return RunActionAsync(() => RunProcessVoid(command, invalidExitCodeMessage, validExitCode));
        }

        public Task RunProcessVoidAsync(IShellCommandBuilder builder, string invalidExitCodeMessage = "Invalid exit code for process.", int validExitCode = 0)
        {
            return RunActionAsync(() => RunProcessVoid(builder, invalidExitCodeMessage, validExitCode));
        }

        public IShellCommandBuilder CreateCommandBuilder()
        {
            return new PwshCommandBuilder();
        }

        private Task<T> RunFuncAsync<T>(Func<T> func)
        {
            var task = new TaskCompletionSource<T>();

            ThreadPool.QueueUserWorkItem((s) =>
            {
                try
                {
                    var result = func.Invoke();
                    task.SetResult(result);
                }
                catch (Exception ex)
                {
                    task.SetException(ex);
                }
            });

            return task.Task;
        }

        private Task RunActionAsync(Action func)
        {
            var task = new TaskCompletionSource<object>();

            ThreadPool.QueueUserWorkItem((s) =>
            {
                try
                {
                    func.Invoke();
                    task.SetResult(func);
                }
                catch (Exception ex)
                {
                    task.SetException(ex);
                }
            });

            return task.Task;
        }
    }

    public class PowershellCoreRunner : PowershellCoreRunner<IShellRunner>, IShellRunner
    {
        public PowershellCoreRunner(IProcessRunnerFactory processRunnerFactory) : base(processRunnerFactory)
        {
        }
    }
}
