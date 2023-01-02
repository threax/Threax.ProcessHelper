using System.Diagnostics;

namespace Threax.ProcessHelper;

public interface IProcessRunner
{
    /// <summary>
    /// Run a task. The error and ouput streams will be redirected and UseShellExectue is forced to false when running
    /// ProcessStartInfo instances this way.
    /// </summary>
    /// <param name="startInfo"></param>
    /// <param name="events"></param>
    /// <returns></returns>
    int Run(ProcessStartInfo startInfo, ProcessEvents? events = null);
}