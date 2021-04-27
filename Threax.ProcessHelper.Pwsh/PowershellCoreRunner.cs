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
        private readonly IObjectPropertyFinder objectPropertyFinder;
        private readonly IPwshArgumentBuilder pwshArgumentBuilder;

        public PowershellCoreRunner(IProcessRunnerFactory<PowershellCoreRunner<T>> processRunnerFactory, IObjectPropertyFinder objectPropertyFinder, IPwshArgumentBuilder pwshArgumentBuilder)
        {
            this.processRunnerFactory = processRunnerFactory;
            this.objectPropertyFinder = objectPropertyFinder;
            this.pwshArgumentBuilder = pwshArgumentBuilder;
        }

        public int RunProcessVoid(FormattableString command)
        {
            var runner = new ExitCodeReaderProcessRunner(processRunnerFactory.Create());

            var escapedCommand = command.GetPwshEnvString(out var args);
            var finalCommand = $"{escapedCommand}; $LASTEXITCODE";
            var startInfo = SetupArgs(finalCommand, args);

            runner.Run(startInfo);

            return runner.LastExitCode;
        }

        public TResult? RunProcess<TResult>(FormattableString command)
        {
            return RunProcess<TResult>(command, out _);
        }

        public TResult? RunProcess<TResult>(FormattableString command, out int exitCode)
        {
            var result = RunProcess(command, out exitCode);
            return result.ToObject<TResult>();
        }

        public JToken RunProcess(FormattableString command)
        {
            return RunProcess(command, out _);
        }

        public JToken RunProcess(FormattableString command, out int exitCode)
        {
            var jsonRunner = new JsonOutputProcessRunner(processRunnerFactory.Create());
            var runner = new ExitCodeReaderProcessRunner(jsonRunner);

            var escapedCommand = command.GetPwshEnvString(out var args);

            var jsonStart = EscapePwshSingleQuote(jsonRunner.JsonStart);
            var jsonEnd = EscapePwshSingleQuote(jsonRunner.JsonEnd);
            var finalCommand = $"'{jsonStart}'; {escapedCommand}; '{jsonEnd}'; $LASTEXITCODE";

            var startInfo = SetupArgs(finalCommand, args);

            runner.Run(startInfo);

            exitCode = runner.LastExitCode;
            return jsonRunner.GetResult();
        }

        //public int RunProcessVoid(String command, Object? args = null)
        //{
        //    var runner = new ExitCodeReaderProcessRunner(processRunnerFactory.Create());

        //    var argString = pwshArgumentBuilder.GetPwshArguments(args);
        //    var finalCommand = $"{command}{argString}; $LASTEXITCODE";
        //    var startInfo = SetupArgs(finalCommand, args);

        //    runner.Run(startInfo);

        //    return runner.LastExitCode;
        //}

        //public TResult? RunProcess<TResult>(String command, Object? args = null)
        //{
        //    return RunProcess<TResult>(command, args, out _);
        //}

        //public TResult? RunProcess<TResult>(String command, Object? args, out int exitCode)
        //{
        //    var result = RunProcess(command, args, out exitCode);
        //    return result.ToObject<TResult>();
        //}

        //public JToken RunProcess(String command, Object? args = null)
        //{
        //    return RunProcess(command, args, out _);
        //}

        //public JToken RunPipeline(String command, Object? args, out int exitCode)
        //{
        //    var jsonRunner = new JsonOutputProcessRunner(processRunnerFactory.Create());
        //    var runner = new ExitCodeReaderProcessRunner(jsonRunner);

        //    var argString = pwshArgumentBuilder.GetPwshArguments(args);
        //    var jsonStart = EscapePwshSingleQuote(jsonRunner.JsonStart);
        //    var jsonEnd = EscapePwshSingleQuote(jsonRunner.JsonEnd);
        //    var finalCommand = $"'{jsonStart}'; {command}{argString}; '{jsonEnd}'; $LASTEXITCODE";

        //    var startInfo = SetupArgs(finalCommand, args);

        //    runner.Run(startInfo);

        //    exitCode = runner.LastExitCode;
        //    return jsonRunner.GetResult();
        //}

        //public int RunCommandVoid(IPwshCommandBuilder command, Object? args = null)
        //{
        //    var runner = processRunnerFactory.Create();
        //    var finalCommand = command.BuildOneLineCommand();
        //    var startInfo = SetupArgs(finalCommand, args);
        //    return runner.Run(startInfo);
        //}

        //public TResult? RunCommand<TResult>(IPwshCommandBuilder command, Object? args = null, int maxDepth = 10)
        //{
        //    return RunCommand<TResult>(command, args, maxDepth, out _);
        //}

        //public TResult? RunCommand<TResult>(IPwshCommandBuilder command, Object? args, int maxDepth, out int exitCode)
        //{
        //    var result = RunCommand(command, args, maxDepth, out exitCode);
        //    return result.ToObject<TResult>();
        //}

        //public JToken RunCommand(IPwshCommandBuilder command, Object? args = null, int maxDepth = 10)
        //{
        //    return RunCommand(command, args, maxDepth, out _);
        //}

        //public JToken RunCommand(IPwshCommandBuilder command, Object? args, int maxDepth, out int exitCode)
        //{
        //    var jsonRunner = new JsonOutputProcessRunner(processRunnerFactory.Create());
        //    jsonRunner.StartWithSkipLines.Add("WARNING: Resulting JSON is truncated as serialization has exceeded the set depth of");

        //    var finalCommand = command.BuildOneLineCommand();
        //    var jsonStart = EscapePwshSingleQuote(jsonRunner.JsonStart);
        //    var jsonEnd = EscapePwshSingleQuote(jsonRunner.JsonEnd);
        //    finalCommand += $"; '{jsonStart}'; $threax_result | ConvertTo-Json -Depth {maxDepth}; '{jsonEnd}';";

        //    var startInfo = SetupArgs(finalCommand, args);

        //    jsonRunner.Run(startInfo);

        //    exitCode = jsonRunner.LastExitCode;
        //    return jsonRunner.GetResult();
        //}

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
