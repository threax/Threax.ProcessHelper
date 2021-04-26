namespace Threax.ProcessHelper.Pwsh
{
    public interface IPwshCommandBuilder
    {
        void AddCommand(string command);
        void AddCommand(string command, object? args);
        void AddResultCommand(string command);
        void AddResultCommand(string command, object? args);
        string BuildOneLineCommand();
    }
}