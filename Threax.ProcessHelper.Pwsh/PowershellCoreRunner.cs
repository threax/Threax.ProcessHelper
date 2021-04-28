using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Threax.ProcessHelper.Pwsh
{
    public class PowershellCoreRunner<T> : IShellRunner<T>
    {
        private readonly IProcessRunnerFactory<T> processRunnerFactory;

        public PowershellCoreRunner(IProcessRunnerFactory<T> processRunnerFactory)
        {
            this.processRunnerFactory = processRunnerFactory;
        }

        public void RunProcessVoid(IEnumerable<FormattableString> command, int validExitCode = 0, string invalidExitCodeMessage = "Invalid exit code for process.")
        {
            var escapedCommand = command.GetPwshEnvString(out var args);
            DoRunProcessVoid(escapedCommand, args, validExitCode, invalidExitCodeMessage);
        }

        public void RunProcessVoid(FormattableString command, int validExitCode = 0, string invalidExitCodeMessage = "Invalid exit code for process.")
        {
            var escapedCommand = command.GetPwshEnvString(out var args);
            DoRunProcessVoid(escapedCommand, args, validExitCode, invalidExitCodeMessage);
        }

        private void DoRunProcessVoid(String escapedCommand, IEnumerable<KeyValuePair<string, object?>> args, int validExitCode, string invalidExitCodeMessage)
        {
            var runner = processRunnerFactory.Create();
            var finalCommand = $"{escapedCommand};exit $LASTEXITCODE";
            var startInfo = SetupArgs(finalCommand, args);
            var exitCode = runner.Run(startInfo);
            if (exitCode != validExitCode)
            {
                throw new InvalidOperationException($"Invalid exit code '{exitCode}' expected '{validExitCode}'. Message: '{invalidExitCodeMessage}'");
            }
        }

        public TResult? RunProcess<TResult>(IEnumerable<FormattableString> command, int validExitCode = 0, String invalidExitCodeMessage = "Invalid exit code for process.")
        {
            var escapedCommand = command.GetPwshEnvString(out var args);
            var result = DoRunProcess(escapedCommand, args, validExitCode, invalidExitCodeMessage);
            return result.ToObject<TResult>();
        }

        public TResult? RunProcess<TResult>(FormattableString command, int validExitCode = 0, String invalidExitCodeMessage = "Invalid exit code for process.")
        {
            var escapedCommand = command.GetPwshEnvString(out var args);
            var result = DoRunProcess(escapedCommand, args, validExitCode, invalidExitCodeMessage);
            return result.ToObject<TResult>();
        }

        public JToken RunProcess(IEnumerable<FormattableString> command, int validExitCode = 0, String invalidExitCodeMessage = "Invalid exit code for process.")
        {
            var escapedCommand = command.GetPwshEnvString(out var args);
            var result = DoRunProcess(escapedCommand, args, validExitCode, invalidExitCodeMessage);
            return result;
        }

        public JToken RunProcess(FormattableString command, int validExitCode = 0, String invalidExitCodeMessage = "Invalid exit code for process.")
        {
            var escapedCommand = command.GetPwshEnvString(out var args);
            var result = DoRunProcess(escapedCommand, args, validExitCode, invalidExitCodeMessage);
            return result;
        }

        private JToken DoRunProcess(String escapedCommand, IEnumerable<KeyValuePair<string, object?>> args, int validExitCode, String invalidExitCodeMessage)
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

        public void RunProcessVoid(IShellCommandBuilder builder, int validExitCode = 0, string invalidExitCodeMessage = "Invalid exit code for process.")
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

        public TResult? RunProcess<TResult>(IShellCommandBuilder builder, int validExitCode = 0, string invalidExitCodeMessage = "Invalid exit code for process.")
        {
            var result = RunProcess(builder, validExitCode, invalidExitCodeMessage);
            return result.ToObject<TResult>();
        }

        public JToken RunProcess(IShellCommandBuilder builder, int validExitCode = 0, string invalidExitCodeMessage = "Invalid exit code for process.")
        {
            var runner = new JsonOutputProcessRunner(processRunnerFactory.Create());
            var jsonStart = EscapePwshSingleQuote(runner.JsonStart);
            var jsonEnd = EscapePwshSingleQuote(runner.JsonEnd);

            var escapedCommand = builder.CreateFinalEscapedCommand(out var args);
            var finalCommand = $"{escapedCommand};${builder.ResultVariableName} = ${builder.ResultVariableName} | ConvertTo-Json -Depth {builder.JsonDepth};'{jsonStart}';${builder.ResultVariableName};'{jsonEnd}'";
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
    }
}
