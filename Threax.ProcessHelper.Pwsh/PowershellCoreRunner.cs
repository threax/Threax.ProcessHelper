using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Threax.ProcessHelper.Pwsh
{
    public class PowershellCoreRunner<T> : IPowershellCoreRunner<T>
    {
        private const String EnvPrefix = "THREAX_";

        private readonly IProcessRunnerFactory<PowershellCoreRunner<T>> processRunnerFactory;
        private readonly IObjectPropertyFinder<T> objectPropertyFinder;

        public PowershellCoreRunner(IProcessRunnerFactory<PowershellCoreRunner<T>> processRunnerFactory, IObjectPropertyFinder<T> objectPropertyFinder)
        {
            this.processRunnerFactory = processRunnerFactory;
            this.objectPropertyFinder = objectPropertyFinder;
        }

        public TResult? RunCommand<TResult>(String command, Object? args = null)
        {
            var commandArgs = GetPwshArguments(args);

            var startInfo = new ProcessStartInfo("pwsh", $"-c '{command}{commandArgs}'");
            if (args != null)
            {
                foreach (var property in objectPropertyFinder.GetObjectProperties(args))
                {
                    startInfo.Environment[$"{EnvPrefix}{property.Key}"] = property.Value;
                }
            }
            var jsonRunner = new JsonOutputProcessRunner(processRunnerFactory.Create());

            jsonRunner.Run(startInfo);

            return jsonRunner.GetResult<TResult>();
        }

        private String GetPwshArguments(object? args)
        {
            if (args == null)
            {
                return "";
            }

            var sb = new StringBuilder();
            foreach (var prop in objectPropertyFinder.GetObjectProperties(args))
            {
                sb.Append($" -{prop.Key} $env:{EnvPrefix}{prop.Key}");
            }

            return sb.ToString();
        }
    }
}
