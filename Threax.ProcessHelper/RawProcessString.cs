using System;
using System.Collections.Generic;
using System.Text;

namespace Threax.ProcessHelper
{
    /// <summary>
    /// Wrapping a value in this class in a FormattedString passed to the IShellRunner will cause it
    /// to be used as is in the final command. Be VERY careful doing this with dynamic values. If one
    /// of them contains evil shell code it will be executed. This should be safe to use with static values
    /// like the names of properties on a c# class that do not change. Use this when sending a string value
    /// as an environment variable won't work, but know that all protections are removed.
    /// </summary>
    public class RawProcessString
    {
        public RawProcessString(String value)
        {
            Value = value;
        }

        public string Value { get; }
    }
}
