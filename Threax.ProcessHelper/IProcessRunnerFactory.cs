namespace Threax.ProcessHelper
{
    public interface IProcessRunnerFactory<T>
    {
        IProcessRunner Create();
    }

    public interface IProcessRunnerFactory : IProcessRunnerFactory<IShellRunner>
    {

    }
}