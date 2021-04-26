namespace Threax.ProcessHelper.Pwsh
{
    public interface IPowershellCoreRunner<T>
    {
        int RunCommand(IPwshCommandBuilder command, object? args = null);
        int RunProcess(string command, object? args = null);
        TResult? RunCommand<TResult>(IPwshCommandBuilder command, object? args = null, int maxDepth = 10);
        TResult? RunCommand<TResult>(IPwshCommandBuilder command, object? args, int maxDepth, out int exitCode);
        TResult? RunProcess<TResult>(string command, object? args);
        TResult? RunProcess<TResult>(string command, object? args, out int exitCode);
    }
}