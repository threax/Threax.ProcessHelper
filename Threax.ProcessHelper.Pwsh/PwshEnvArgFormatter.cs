using System;
using System.Collections.Generic;
using System.Text;

namespace Threax.ProcessHelper.Pwsh
{
    static class PwshEnvArgFormatter
    {
        public static String GetPwshEnvString(this IEnumerable<FormattableString> str, out IEnumerable<KeyValuePair<String, Object?>> args, string prefix = "")
        {
            var sb = new StringBuilder();
            var formatter = new EnvArgFormatter(prefix);
            var formatProvider = new EnvArgFormatProvider(formatter);
            foreach (var i in str)
            {
                sb.AppendFormat(formatProvider, i.Format, i.GetArguments());
            }
            args = formatter.GetEnvArgs();
            return sb.ToString();
        }

        public static String GetPwshEnvString(this FormattableString str, out IEnumerable<KeyValuePair<String, Object?>> args, string prefix = "")
        {
            var formatter = new EnvArgFormatter(prefix);
            var result = str.ToString(new EnvArgFormatProvider(formatter));
            args = formatter.GetEnvArgs();
            return result;
        }

        class EnvArgFormatProvider : IFormatProvider
        {
            private readonly EnvArgFormatter formatter;

            public EnvArgFormatProvider(EnvArgFormatter formatter)
            {
                this.formatter = formatter;
            }

            public object? GetFormat(Type? formatType)
            {
                if (formatType == typeof(ICustomFormatter))
                {
                    return formatter;
                }
                return null;
            }
        }

        class EnvArgFormatter : ICustomFormatter
        {
            private readonly List<object?> values = new List<object?>();
            private readonly string prefix;

            public EnvArgFormatter(String prefix)
            {
                this.prefix = prefix;
            }

            public IEnumerable<KeyValuePair<String, Object?>> GetEnvArgs()
            {
                var numValues = values.Count;
                for(var i = 0; i < numValues; ++i)
                {
                    var key = GetName(i);
                    var value = values[i];
                    yield return new KeyValuePair<string, object?>(key, value);
                }
            }

            public string Format(string? format, object? arg, IFormatProvider? formatProvider)
            {
                var rawArg = arg as RawProcessString;
                if (rawArg != null)
                {
                    return rawArg.Value;
                }
                else
                {
                    var index = values.Count;
                    values.Add(arg);
                    return GetEnvArg(index);
                }
            }

            private String GetEnvArg(int index)
            {
                return $"${{env:{GetName(index)}}}";
            }

            private String GetName(int index)
            {
                return $"pwsh_arg_{prefix}{index}";
            }
        }
    }
}
