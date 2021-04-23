using System.Collections.Generic;

namespace Threax.ProcessHelper
{
    public interface IObjectPropertyFinder<T>
    {
        IEnumerable<KeyValuePair<string, string?>> GetObjectProperties(object args);
    }
}