using System;
using System.Collections.Generic;
using System.Text;

namespace Threax.ProcessHelper
{
    public class ProcessRunnerFactory<T> : IProcessRunnerFactory<T>
    {
        public IProcessRunner Create()
        {
            return new ProcessRunner();
        }
    }

    public class ProcessRunnerFactory : ProcessRunnerFactory<ProcessRunnerFactory>, IProcessRunnerFactory
    {

    }
}
