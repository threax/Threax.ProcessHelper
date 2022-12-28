using System.Diagnostics;

namespace Threax.ProcessHelper
{
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

    /// <summary>
    /// A process runner that includes logged output scoped to the given type.
    /// </summary>
    /// <typeparam name="TLog"></typeparam>
    public interface IProcessRunner<TLog> : IProcessRunner
    {

    }
}