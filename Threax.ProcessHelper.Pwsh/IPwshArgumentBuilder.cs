namespace Threax.ProcessHelper.Pwsh
{
    public interface IPwshArgumentBuilder
    {
        string GetPwshArguments(object? args);

        string GetEnvVarName(string name);
        
        /// <summary>
        /// The way that arguments should be called. Defaults to powershell '-', but can be changed to run
        /// external commands.
        /// </summary>
        public string ArgumentCallStyle { get; set; }
    }

    public interface IPwshArgumentBuilder<T> : IPwshArgumentBuilder
    {

    }
}