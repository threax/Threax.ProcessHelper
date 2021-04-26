using System.Collections.Generic;

namespace Threax.ProcessHelper
{
    public interface IObjectPropertyFinder
    {
        IEnumerable<KeyValuePair<string, string?>> GetObjectProperties(object args);
    }
}