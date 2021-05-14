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

        void AddCommand(FormattableString command);
        void AddCommand(IEnumerable<FormattableString> command);

        void AddResultCommand(FormattableString command);
        void AddResultCommand(IEnumerable<FormattableString> command);

        string CreateFinalEscapedCommand(out IEnumerable<KeyValuePair<string, object?>> args);
    }
}