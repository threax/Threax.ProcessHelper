using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Threax.ProcessHelper.Pwsh
{
    public class PwshCommandBuilder : IShellCommandBuilder
    {
        private readonly List<IEnumerable<FormattableString>> commands = new List<IEnumerable<FormattableString>>();
        private int resultLineIndex = -1;

        public PwshCommandBuilder()
        {

        }

        public String ResultVariableName { get; set; } = "threax_result";

        public int JsonDepth { get; set; } = 10;

        public void AddCommand(FormattableString command)
        {
            AddCommand(new FormattableString[] { command });
        }

        public void AddCommand(IEnumerable<FormattableString> command)
        {
            commands.Add(command);
        }

        public void AddResultCommand(FormattableString command)
        {
            AddResultCommand(new FormattableString[] { command });
        }

        public void AddResultCommand(IEnumerable<FormattableString> command)
        {
            if (resultLineIndex != -1)
            {
                throw new InvalidOperationException("Can only add 1 result line per command.");
            }

            resultLineIndex = commands.Count;
            commands.Add(command);
        }

        public String CreateFinalEscapedCommand(out IEnumerable<KeyValuePair<String, Object?>> args)
        {
            if(resultLineIndex == -1)
            {
                throw new InvalidOperationException("You must include a result line to run a pwsh command.");
            }

            var sb = new StringBuilder();
            var sep = "";
            args = Enumerable.Empty<KeyValuePair<String, Object?>>();
            int index = 0;
            foreach (var command in commands)
            {
                var prefix = $"l{index}_";
                var finalCommand = command.GetPwshEnvString(out var lineArgs, prefix);
                if(index == resultLineIndex)
                {
                    finalCommand = $"${ResultVariableName} = {finalCommand}";
                }
                args = args.Concat(lineArgs);

                sb.Append(sep);
                sb.Append(finalCommand);
                sep = "; ";

                ++index;
            }
            return sb.ToString();
        }
    }
}
