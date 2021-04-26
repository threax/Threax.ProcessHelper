using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Threax.ProcessHelper
{
    public class ObjectPropertyFinder : IObjectPropertyFinder
    {
        public IEnumerable<KeyValuePair<string, string?>> GetObjectProperties(Object args)
        {
            var argType = args.GetType();
            var callArgs = new object[0];
            foreach (var property in argType.GetProperties())
            {
                var getMethod = property.GetMethod;
                if (getMethod != null)
                {
                    var value = getMethod.Invoke(args, callArgs)?.ToString();
                    yield return new KeyValuePair<string, string?>(property.Name, value);
                }
            }
        }
    }
}
