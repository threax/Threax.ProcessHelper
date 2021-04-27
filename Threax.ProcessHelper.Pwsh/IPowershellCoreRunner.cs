using Newtonsoft.Json.Linq;

namespace Threax.ProcessHelper.Pwsh
{
    public interface IPowershellCoreRunner<T>
    {
        int RunCommandVoid(IPwshCommandBuilder command, object? args = null);
        TResult? RunCommand<TResult>(IPwshCommandBuilder command, object? args = null, int maxDepth = 10);
        TResult? RunCommand<TResult>(IPwshCommandBuilder command, object? args, int maxDepth, out int exitCode);
        int RunProcessVoid(string command, object? args = null);
        TResult? RunProcess<TResult>(string command, object? args = null);
        TResult? RunProcess<TResult>(string command, object? args, out int exitCode);
        JToken RunProcess(string command, object? args = null);
        JToken RunProcess(string command, object? args, out int exitCode);
    }
}