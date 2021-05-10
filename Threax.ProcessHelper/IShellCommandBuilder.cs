using System;
using System.Collections.Generic;

namespace Threax.ProcessHelper
{
    public interface IShellCommandBuilder
    {
        /// <summary>
        /// The name to use for the result variable.
        /// </summary>
        string ResultVariableName { get; set; }

        /// <summary>
        /// The depth of json to convert if supported by the underlying shell. Default: 10
        /// </summary>
        int JsonDepth { get; set; }

        void AddCommand(FormattableString command);
        void AddCommand(IEnumerable<FormattableString> command);

        void AddResultCommand(FormattableString command);
        void AddResultCommand(IEnumerable<FormattableString> command);

        string CreateFinalEscapedCommand(out IEnumerable<KeyValuePair<string, object?>> args);
    }
}