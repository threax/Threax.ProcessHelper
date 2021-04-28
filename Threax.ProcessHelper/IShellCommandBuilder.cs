using System;
using System.Collections.Generic;

namespace Threax.ProcessHelper
{
    public interface IShellCommandBuilder<T>
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

        void AddResultCommand(FormattableString command);
        
        string CreateFinalEscapedCommand(out IEnumerable<KeyValuePair<string, object?>> args);
    }

    public interface IShellCommandBuilder : IShellCommandBuilder<IShellCommandBuilder>
    {

    }
}