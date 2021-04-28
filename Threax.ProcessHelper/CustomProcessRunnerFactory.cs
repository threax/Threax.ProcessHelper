using System;
using System.Collections.Generic;
using System.Text;

namespace Threax.ProcessHelper
{
    public class CustomProcessRunnerFactory<T> : IProcessRunnerFactory<T>
    {
        private readonly Func<IProcessRunner> create;

        public CustomProcessRunnerFactory(Func<IProcessRunner> create)
        {
            this.create = create;
        }

        public IProcessRunner Create()
        {
            return create.Invoke();
        }
    }

    public class CustomProcessRunnerFactory : CustomProcessRunnerFactory<CustomProcessRunnerFactory>, IProcessRunnerFactory
    {
        public CustomProcessRunnerFactory(Func<IProcessRunner> create) : base(create)
        {
        }
    }
}
