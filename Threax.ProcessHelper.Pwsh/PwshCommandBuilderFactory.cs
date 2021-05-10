using System;
using System.Collections.Generic;
using System.Text;

namespace Threax.ProcessHelper.Pwsh
{
    class PwshCommandBuilderFactory : IShellCommandBuilderFactory
    {
        public IShellCommandBuilder Create()
        {
            return new PwshCommandBuilder();
        }
    }

    class PwshCommandBuilderFactory<T> : PwshCommandBuilderFactory, IShellCommandBuilderFactory<T>
    {
    }
}
