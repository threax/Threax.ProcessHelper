namespace Threax.ProcessHelper.Pwsh
{
    public interface IPowershellCoreRunner<T>
    {
        TResult? RunCommand<TResult>(string command, object? args = null);
    }
}