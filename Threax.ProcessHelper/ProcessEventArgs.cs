using System.Diagnostics;

namespace Threax.ProcessHelper;

public class ProcessEventArgs
{

#nullable disable
    public DataReceivedEventArgs DataReceivedEventArgs { get; internal set; }
#nullable enable

    public bool AllowOutput { get; set; }

    internal void Reset()
    {
        AllowOutput = true;

#nullable disable
        this.DataReceivedEventArgs = null;
#nullable enable
    }
}
