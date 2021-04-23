namespace Threax.ProcessHelper
{
    public interface IProcessRunnerFactory<T>
    {
        IProcessRunner Create();
    }
}