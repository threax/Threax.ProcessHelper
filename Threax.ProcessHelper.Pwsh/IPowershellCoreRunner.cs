using System;

namespace Threax.ProcessHelper.Pwsh
{
    public interface IPowershellCoreRunner<T>
    {
        public TResult? RunCommand<TResult>(String command, Object? args = null, int maxDepth = 100);

        public TResult? RunCommand<TResult>(String command, Object? args, int maxDepth, out int exitCode);
    }
}