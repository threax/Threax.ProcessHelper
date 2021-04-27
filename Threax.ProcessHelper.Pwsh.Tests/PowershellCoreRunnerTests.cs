using System;
using Threax.AspNetCore.Tests;
using Xunit;

namespace Threax.ProcessHelper.Pwsh.Tests
{
    public class PowershellCoreRunnerTests
    {
        Mockup mockup = new Mockup();

        public PowershellCoreRunnerTests()
        {
            mockup.MockServiceCollection.AddThreaxPwshProcessHelper<PowershellCoreRunnerTests>();
        }

        [Fact]
        public void Echo()
        {
            var runner = mockup.Get<IPowershellCoreRunner<PowershellCoreRunnerTests>>();
            var commandBuilder = mockup.Get<IPwshCommandBuilder>();
            commandBuilder.AddResultCommand("'Hi'");
            var result = runner.RunCommand<string>(commandBuilder);
            Assert.Equal("Hi", result);
        }

        class HostInfo
        {
            public String Name { get; set; }
        }

        [Fact]
        public void GetHost()
        {
            var runner = mockup.Get<IPowershellCoreRunner<PowershellCoreRunnerTests>>();
            var commandBuilder = mockup.Get<IPwshCommandBuilder>();
            commandBuilder.AddResultCommand("Get-Host");
            var result = runner.RunCommand<HostInfo>(commandBuilder, maxDepth: 2);
            Assert.Equal("ConsoleHost", result.Name);
        }

        [Fact]
        public void FailMultipleResults()
        {
            var runner = mockup.Get<IPowershellCoreRunner<PowershellCoreRunnerTests>>();
            var commandBuilder = mockup.Get<IPwshCommandBuilder>();
            commandBuilder.AddResultCommand("'Hi'");
            Assert.Throws<InvalidOperationException>(() => commandBuilder.AddResultCommand("'Hi'"));
        }

        [Fact]
        public void FailMultipleResultsArgs()
        {
            var runner = mockup.Get<IPowershellCoreRunner<PowershellCoreRunnerTests>>();
            var commandBuilder = mockup.Get<IPwshCommandBuilder>();
            commandBuilder.AddResultCommand("'Hi'", new { Value = 1 });
            Assert.Throws<InvalidOperationException>(() => commandBuilder.AddResultCommand("'Hi'", new { Value = 1 }));
        }

        [Fact]
        public void RunProcess()
        {
            var runner = mockup.Get<IPowershellCoreRunner<PowershellCoreRunnerTests>>();
            var result = runner.RunProcessVoid("ping threax.com");
            Assert.Equal(0, result);
        }

        [Fact]
        public void RunProcessArgs()
        {
            var runner = mockup.Get<IPowershellCoreRunner<PowershellCoreRunnerTests>>();
            var result = runner.RunProcessVoid("ping threax.com", new { n = 5 });
            Assert.Equal(0, result);
        }

        [Fact]
        public void RunProcessJToken()
        {

            var runner = mockup.Get<IPowershellCoreRunner<PowershellCoreRunnerTests>>();
            dynamic result = runner.RunProcess("[PSCustomObject]@{ Name = 'Test'; Value = 'SomeValue'; } | ConvertTo-Json; $LASTEXITCODE=0");
            Assert.Equal("Test", (string?)result.Name);
        }

        class TestObj
        {
            public String Name { get; set; }

            public String Value { get; set; }
        }

        [Fact]
        public void RunProcessObject()
        {
            var runner = mockup.Get<IPowershellCoreRunner<PowershellCoreRunnerTests>>();
            dynamic result = runner.RunProcess<TestObj>("[PSCustomObject]@{ Name = 'Test'; Value = 'SomeValue'; } | ConvertTo-Json; $LASTEXITCODE=0");
            Assert.Equal("Test", result.Name);
        }
    }
}
