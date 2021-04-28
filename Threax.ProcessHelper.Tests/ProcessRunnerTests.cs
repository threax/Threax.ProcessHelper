using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Diagnostics;
using Threax.AspNetCore.Tests;
using Xunit;

namespace Threax.ProcessHelper.Tests
{
    public class ProcessRunnerTests
    {
        public ProcessRunnerTests()
        {
            
        }

        [Fact]
        public void Echo()
        {
            var processRunner = new ProcessRunner();
            var startInfo = new ProcessStartInfo("pwsh", "-c 'hi'");
            var result = processRunner.Run(startInfo);
            Assert.Equal(0, result);
        }

        [Fact]
        public void EchoError()
        {
            var processRunner = new ProcessRunner();
            var startInfo = new ProcessStartInfo("pwsh", "-c Write-Error 'hi'");
            var result = processRunner.Run(startInfo);
            Assert.Equal(1, result);
        }

        [Fact]
        public void Fail()
        {
            var processRunner = new ProcessRunner();
            var startInfo = new ProcessStartInfo("pwsh", "asdfdsaf");
            var result = processRunner.Run(startInfo);
            Assert.Equal(64, result);
        }
    }
}
