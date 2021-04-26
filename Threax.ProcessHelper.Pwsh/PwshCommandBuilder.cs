using System;
using System.Collections.Generic;
using System.Text;

namespace Threax.ProcessHelper.Pwsh
{
    public class PwshCommandBuilder : IPwshCommandBuilder
    {
        private readonly IPwshArgumentBuilder argumentBuilder;
        private readonly List<String> commands = new List<string>();

        public PwshCommandBuilder(IPwshArgumentBuilder argumentBuilder)
        {
            this.argumentBuilder = argumentBuilder;
        }

        public void AddCommand(String command)
        {
            AddCommand(command, null);
        }

        public void AddCommand(String command, Object? args)
        {
            commands.Add(command + argumentBuilder.GetPwshArguments(args));
        }

        public void AddResultCommand(String command)
        {
            AddResultCommand(command, null);
        }

        public void AddResultCommand(String command, Object? args)
        {
            AddCommand($"$threax_result = {command}", args);
        }

        public String BuildOneLineCommand()
        {
            var sb = new StringBuilder();
            var sep = "";
            foreach (var command in commands)
            {
                sb.Append(sep);
                sb.Append(command);
                sep = "; ";
            }
            return sb.ToString();
        }
    }
}
