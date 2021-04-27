using Newtonsoft.Json.Linq;
using System;

namespace Threax.ProcessHelper.Pwsh
{
    public interface IPowershellCoreRunner<T>
    {
        JToken RunProcess(FormattableString command);
        JToken RunProcess(FormattableString command, out int exitCode);
        TResult? RunProcess<TResult>(FormattableString command);
        TResult? RunProcess<TResult>(FormattableString command, out int exitCode);
        int RunProcessVoid(FormattableString command);
    }
}