using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Diagnostics;
using Threax.AspNetCore.Tests;
using Xunit;

namespace Threax.ProcessHelper.Tests
{
    public class LoggingProcessRunnerTests
    {
        Mockup mockup = new Mockup();

        public LoggingProcessRunnerTests()
        {
            var mockLogger = new Mock<ILogger<LoggingProcessRunner<ProcessRunnerTests>>>();
            mockup.Add(m => mockLogger.Object);

            mockup.MockServiceCollection.AddThreaxProcessHelper<ProcessRunnerTests>(o =>
            {
                o.SetupRunner = (r, s) => new LoggingProcessRunner<ProcessRunnerTests>(r, s.GetRequiredService<ILogger<LoggingProcessRunner<ProcessRunnerTests>>>());
            });
        }

        [Fact]
        public void Echo()
        {
            var factory = mockup.Get<IProcessRunnerFactory<ProcessRunnerTests>>();
            var processRunner = factory.Create();
            var startInfo = new ProcessStartInfo("pwsh", "-c 'hi'");
            var result = processRunner.Run(startInfo);
            Assert.Equal(0, result);
        }

        [Fact]
        public void EchoError()
        {
            var factory = mockup.Get<IProcessRunnerFactory<ProcessRunnerTests>>();
            var processRunner = factory.Create();
            var startInfo = new ProcessStartInfo("pwsh", "-c Write-Error 'hi'");
            var result = processRunner.Run(startInfo);
            Assert.Equal(1, result);
        }

        [Fact]
        public void Fail()
        {
            var factory = mockup.Get<IProcessRunnerFactory<ProcessRunnerTests>>();
            var processRunner = factory.Create();
            var startInfo = new ProcessStartInfo("pwsh", "asdfasdfdsa");
            var result = processRunner.Run(startInfo);
            Assert.Equal(64, result);
        }
    }
}
