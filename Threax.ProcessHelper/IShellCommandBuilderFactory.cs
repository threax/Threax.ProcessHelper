using System;
using System.Collections.Generic;
using System.Text;

namespace Threax.ProcessHelper
{
    public interface IShellCommandBuilderFactory
    {
        public IShellCommandBuilder Create();
    }

    public interface IShellCommandBuilderFactory<T> : IShellCommandBuilderFactory
    {

    }
}
