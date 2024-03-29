using Microsoft.Extensions.Logging;
using System.Diagnostics;
using Threax.AspNetCore.Tests;
using Xunit;

namespace Threax.ProcessHelper.Tests;

public class LoggingProcessRunnerTests
{
    Mockup mockup = new Mockup();

    public LoggingProcessRunnerTests()
    {
        
    }

    [Fact]
    public void Echo()
    {
        var processRunner = new LoggingProcessRunner(new ProcessRunner(), mockup.Get<ILogger<LoggingProcessRunnerTests>>());
        var startInfo = new ProcessStartInfo("pwsh", "-c 'hi'");
        var result = processRunner.Run(startInfo);
        Assert.Equal(0, result);
    }

    [Fact]
    public void EchoError()
    {
        var processRunner = new LoggingProcessRunner(new ProcessRunner(), mockup.Get<ILogger<LoggingProcessRunnerTests>>());
        var startInfo = new ProcessStartInfo("pwsh", "-c Write-Error 'hi'");
        var result = processRunner.Run(startInfo);
        Assert.Equal(1, result);
    }

    [Fact]
    public void Fail()
    {
        var processRunner = new LoggingProcessRunner(new ProcessRunner(), mockup.Get<ILogger<LoggingProcessRunnerTests>>());
        var startInfo = new ProcessStartInfo("pwsh", "asdfasdfdsa");
        var result = processRunner.Run(startInfo);
        Assert.Equal(64, result);
    }
}
