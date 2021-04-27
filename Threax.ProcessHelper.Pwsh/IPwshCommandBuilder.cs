using System;
using System.Collections.Generic;

namespace Threax.ProcessHelper.Pwsh
{
    public interface IPwshCommandBuilder
    {
        /// <summary>
        /// The name to use for the result variable.
        /// </summary>
        string ResultVariableName { get; set; }

        /// <summary>
        /// The depth of json to convert with ConvertTo-Json -Depth. Default: 10
        /// </summary>
        int JsonDepth { get; set; }

        void AddCommand(FormattableString command);
        void AddResultCommand(FormattableString command);
        string BuildOneLineCommand(out IEnumerable<KeyValuePair<string, object?>> args);
    }
}