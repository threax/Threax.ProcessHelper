using System;
using System.Collections.Generic;
using System.Text;

namespace Threax.ProcessHelper.Pwsh
{
    class PwshArgumentBuilder : IPwshArgumentBuilder
    {
        private const String EnvPrefix = "THREAX_";
        private readonly IObjectPropertyFinder objectPropertyFinder;

        public PwshArgumentBuilder(IObjectPropertyFinder objectPropertyFinder)
        {
            this.objectPropertyFinder = objectPropertyFinder;
        }

        public String GetPwshArguments(object? args)
        {
            if (args == null)
            {
                return "";
            }

            var sb = new StringBuilder();
            foreach (var prop in objectPropertyFinder.GetObjectProperties(args))
            {
                sb.Append($" -{prop.Key} $env:{GetEnvVarName(prop.Key)}");
            }

            return sb.ToString();
        }

        public String GetEnvVarName(String name)
        {
            return EnvPrefix + name;
        }
    }
}
