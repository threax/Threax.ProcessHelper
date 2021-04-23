using System.Diagnostics;

namespace Threax.ProcessHelper
{
    public interface IProcessRunner
    {
        int Run(ProcessStartInfo startInfo, ProcessEvents? events = null);
    }
}