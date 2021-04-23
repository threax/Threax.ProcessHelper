using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Threax.ProcessHelper
{
    public class ObjectPropertyFinder<T> : IObjectPropertyFinder<T>
    {
        public IEnumerable<KeyValuePair<string, string?>> GetObjectProperties(Object args)
        {
            var argType = args.GetType();
            var callArgs = new object[0];
            foreach (var getMethod in argType.GetProperties().Select(i => i.GetMethod))
            {
                if (getMethod != null)
                {
                    var value = getMethod.Invoke(args, callArgs)?.ToString();
                    yield return new KeyValuePair<string, string?>(getMethod.Name, value);
                }
            }
        }
    }
}
