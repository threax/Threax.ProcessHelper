namespace Threax.ProcessHelper.Pwsh
{
    public interface IPwshArgumentBuilder
    {
        string GetPwshArguments(object? args);

        string GetEnvVarName(string name);
    }
}