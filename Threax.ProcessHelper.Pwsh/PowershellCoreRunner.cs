using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Threax.ProcessHelper.Pwsh
{
    public class PowershellCoreRunner<T> : IPowershellCoreRunner<T>
    {
        private readonly IProcessRunnerFactory<PowershellCoreRunner<T>> processRunnerFactory;

        public PowershellCoreRunner(IProcessRunnerFactory<PowershellCoreRunner<T>> processRunnerFactory)
        {
            this.processRunnerFactory = processRunnerFactory;
        }

        public int RunProcessVoid(FormattableString command)
        {
            var runner = processRunnerFactory.Create();
            var escapedCommand = command.GetPwshEnvString(out var args);
            var finalCommand = $"{escapedCommand};exit $LASTEXITCODE";
            var startInfo = SetupArgs(finalCommand, args);
            var exitCode = runner.Run(startInfo);
            return exitCode;
        }

        public TResult? RunProcess<TResult>(FormattableString command, int validExitCode = 0, String invalidExitCodeMessage = "Invalid exit code for process.")
        {
            var result = RunProcess(command, validExitCode, invalidExitCodeMessage);
            return result.ToObject<TResult>();
        }

        public JToken RunProcess(FormattableString command, int validExitCode = 0, String invalidExitCodeMessage = "Invalid exit code for process.")
        {
            DoRunProcess(command, validExitCode, invalidExitCodeMessage, out var runner);

            return runner.GetResult();
        }

        private void DoRunProcess(FormattableString command, int validExitCode, String invalidExitCodeMessage, out JsonOutputProcessRunner runner)
        {
            runner = new JsonOutputProcessRunner(processRunnerFactory.Create());
            var jsonStart = EscapePwshSingleQuote(runner.JsonStart);
            var jsonEnd = EscapePwshSingleQuote(runner.JsonEnd);

            var escapedCommand = command.GetPwshEnvString(out var args);
            var finalCommand = $"'{jsonStart}';{escapedCommand};'{jsonEnd}';exit $LASTEXITCODE";
            var startInfo = SetupArgs(finalCommand, args);
            var exitCode = runner.Run(startInfo);
            if (exitCode != validExitCode)
            {
                throw new InvalidOperationException($"Invalid exit code '{exitCode}' expected '{validExitCode}'. Message: '{invalidExitCodeMessage}'");
            }
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
