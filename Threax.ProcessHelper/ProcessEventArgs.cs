using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Threax.ProcessHelper
{
    public class ProcessEventArgs
    {

#pragma warning disable 8618 //This is null while its in the pool, but never externally
        public DataReceivedEventArgs DataReceivedEventArgs { get; internal set; }
#pragma warning restore 8618

        public bool AllowOutput { get; set; }

        internal void Reset()
        {
            AllowOutput = true;

#pragma warning disable 8625 //This will only be null here, never passed externally
            this.DataReceivedEventArgs = null;
#pragma warning restore 8625
        }
    }
}
