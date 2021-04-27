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

        //[Fact]
        //public void Echo()
        //{
        //    var runner = mockup.Get<IPowershellCoreRunner<PowershellCoreRunnerTests>>();
        //    var result = runner.RunProcess<string>($"'Hi'");
        //    Assert.Equal("Hi", result);
        //}

        //class HostInfo
        //{
        //    public String Name { get; set; }
        //}

        //[Fact]
        //public void GetHost()
        //{
        //    var runner = mockup.Get<IPowershellCoreRunner<PowershellCoreRunnerTests>>();
        //    var result = runner.RunProcess<HostInfo>($"Get-Host", jsonDepth: 2);
        //    Assert.Equal("ConsoleHost", result.Name);
        //}

        //[Fact]
        //public void FailMultipleResults()
        //{
        //    var runner = mockup.Get<IPowershellCoreRunner<PowershellCoreRunnerTests>>();
        //    var commandBuilder = mockup.Get<IPwshCommandBuilder>();
        //    commandBuilder.AddResultCommand("'Hi'");
        //    Assert.Throws<InvalidOperationException>(() => commandBuilder.AddResultCommand("'Hi'"));
        //}

        //[Fact]
        //public void FailMultipleResultsArgs()
        //{
        //    var runner = mockup.Get<IPowershellCoreRunner<PowershellCoreRunnerTests>>();
        //    var commandBuilder = mockup.Get<IPwshCommandBuilder>();
        //    commandBuilder.AddResultCommand("'Hi'", new { Value = 1 });
        //    Assert.Throws<InvalidOperationException>(() => commandBuilder.AddResultCommand("'Hi'", new { Value = 1 }));
        //}

        [Fact]
        public void RunProcessVoid()
        {
            var runner = mockup.Get<IPowershellCoreRunner<PowershellCoreRunnerTests>>();
            var numTimes = 5;
            var result = runner.RunProcessVoid($"ping threax.com -n {numTimes}");
            Assert.Equal(0, result);
        }

        [Fact]
        public void RunProcessVoidFail()
        {
            var runner = mockup.Get<IPowershellCoreRunner<PowershellCoreRunnerTests>>();
            var exitCode = runner.RunProcessVoid($"ping");
            Assert.Equal(1, exitCode);
        }

        [Fact]
        public void RunProcessJToken()
        {

            var runner = mockup.Get<IPowershellCoreRunner<PowershellCoreRunnerTests>>();
            var name = "Test";
            var value = "SomeValue";
            dynamic result = runner.RunProcess($"[PSCustomObject]@{{ Name = {name}; Value = {value}; }} | ConvertTo-Json -Depth 2");
            Assert.Equal("Test", (string?)result.Name);
        }

        [Fact]
        public void RunProcessJTokenFail()
        {

            var runner = mockup.Get<IPowershellCoreRunner<PowershellCoreRunnerTests>>();
            var name = "Test";
            var value = "SomeValue";
            Assert.Throws<InvalidOperationException>(() => runner.RunProcess($"[PSCustomObject]@{{ Name = {name}; Value = {value}; }} | ConvertTo-Json -Depth 2; throw;"));
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
            var name = "Test";
            var value = "SomeValue";
            var result = runner.RunProcess<TestObj>($"[PSCustomObject]@{{ Name = {name}; Value = {value}; }} | ConvertTo-Json -Depth 2");
            Assert.Equal("Test", result.Name);
        }

        [Fact]
        public void RunProcessObjectFail()
        {
            var runner = mockup.Get<IPowershellCoreRunner<PowershellCoreRunnerTests>>();
            var name = "Test";
            var value = "SomeValue";
            Assert.Throws<InvalidOperationException>(() => runner.RunProcess<TestObj>($"[PSCustomObject]@{{ Name = {name}; Value = {value}; }} | ConvertTo-Json -Depth 2; throw;"));
        }
    }
}
