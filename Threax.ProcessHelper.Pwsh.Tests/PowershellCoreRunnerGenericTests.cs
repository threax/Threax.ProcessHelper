using Microsoft.Extensions.DependencyInjection;
using System;
using Threax.AspNetCore.Tests;
using Xunit;

namespace Threax.ProcessHelper.Pwsh.Tests
{
    public class PowershellCoreRunnerGenericTests
    {
        Mockup mockup = new Mockup();

        public PowershellCoreRunnerGenericTests()
        {
            mockup.MockServiceCollection.AddLogging();
            mockup.MockServiceCollection.AddThreaxPwshShellRunner<PowershellCoreRunnerTests>();
        }

        [Fact]
        public void RunProcessCommandsVoidSecurityCheck()
        {
            var runner = mockup.Get<IShellRunner<PowershellCoreRunnerTests>>();
            var factory = mockup.Get<IShellCommandBuilderFactory<PowershellCoreRunnerTests>>();
            var builder = factory.Create();
            var evil = "exit 44;";
            builder.AddCommand($"'Before'");
            builder.AddResultCommand($"{evil}");
            builder.AddCommand($"'After'");
            runner.RunProcessVoid(builder);
        }

        [Fact]
        public void RunProcessCommandsVoid()
        {
            var runner = mockup.Get<IShellRunner<PowershellCoreRunnerTests>>();
            var factory = mockup.Get<IShellCommandBuilderFactory<PowershellCoreRunnerTests>>();
            var builder = factory.Create();
            builder.AddCommand($"'Before'");
            builder.AddResultCommand($"'Hi'");
            builder.AddCommand($"'After'");
            runner.RunProcessVoid(builder);
        }

        [Fact]
        public void RunProcessCommandsJTokenSecurityCheck()
        {
            var runner = mockup.Get<IShellRunner<PowershellCoreRunnerTests>>();
            var factory = mockup.Get<IShellCommandBuilderFactory<PowershellCoreRunnerTests>>();
            var builder = factory.Create();
            var evil = "echo hi";
            builder.AddCommand($"'Before'");
            builder.AddResultCommand($"{evil}");
            builder.AddCommand($"'After'");
            var result = runner.RunProcess(builder);
            Assert.Equal("echo hi", result.ToString());
        }

        [Fact]
        public void RunProcessCommandsJToken()
        {
            var runner = mockup.Get<IShellRunner<PowershellCoreRunnerTests>>();
            var factory = mockup.Get<IShellCommandBuilderFactory<PowershellCoreRunnerTests>>();
            var builder = factory.Create();
            builder.AddCommand($"'Before'");
            builder.AddResultCommand($"'Hi'");
            builder.AddCommand($"'After'");
            var result = runner.RunProcess(builder);
            Assert.Equal("Hi", result.ToString());
        }

        [Fact]
        public void RunProcessCommandJTokenArgs()
        {
            var runner = mockup.Get<IShellRunner<PowershellCoreRunnerTests>>();
            var factory = mockup.Get<IShellCommandBuilderFactory<PowershellCoreRunnerTests>>();
            var builder = factory.Create();
            var info = "Hi";
            builder.AddCommand($"'Before'");
            builder.AddResultCommand($"{info}");
            builder.AddCommand($"'After'");
            var result = runner.RunProcess(builder);
            Assert.Equal("Hi", result.ToString());
        }

        [Fact]
        public void RunProcessCommandsObjectSecurityCheck()
        {
            var runner = mockup.Get<IShellRunner<PowershellCoreRunnerTests>>();
            var factory = mockup.Get<IShellCommandBuilderFactory<PowershellCoreRunnerTests>>();
            var builder = factory.Create();
            var evil = "echo hi";
            builder.AddCommand($"'Before'");
            builder.AddResultCommand($"{evil}");
            builder.AddCommand($"'After'");
            var result = runner.RunProcess<String>(builder);
            Assert.Equal("echo hi", result);
        }

        [Fact]
        public void RunProcessCommandsObject()
        {
            var runner = mockup.Get<IShellRunner<PowershellCoreRunnerTests>>();
            var factory = mockup.Get<IShellCommandBuilderFactory<PowershellCoreRunnerTests>>();
            var builder = factory.Create();
            builder.AddCommand($"'Before'");
            builder.AddResultCommand($"'Hi'");
            builder.AddCommand($"'After'");
            var result = runner.RunProcess<String>(builder);
            Assert.Equal("Hi", result);
        }

        [Fact]
        public void RunProcessCommandArgs()
        {
            var runner = mockup.Get<IShellRunner<PowershellCoreRunnerTests>>();
            var factory = mockup.Get<IShellCommandBuilderFactory<PowershellCoreRunnerTests>>();
            var builder = factory.Create();
            var info = "Hi";
            builder.AddCommand($"'Before'");
            builder.AddResultCommand($"{info}");
            builder.AddCommand($"'After'");
            var result = runner.RunProcess<String>(builder);
            Assert.Equal("Hi", result);
        }

        [Fact]
        public void RunProcessCommandMultiArgsBefore()
        {
            var runner = mockup.Get<IShellRunner<PowershellCoreRunnerTests>>();
            var factory = mockup.Get<IShellCommandBuilderFactory<PowershellCoreRunnerTests>>();
            var builder = factory.Create();
            var info = "Hi";
            var dummy = "Dummy";
            builder.AddCommand($"'Before'");
            builder.AddCommand($"{dummy}");
            builder.AddCommand($"{dummy}");
            builder.AddCommand($"{dummy}");
            builder.AddResultCommand($"{info}");
            builder.AddCommand($"'After'");
            var result = runner.RunProcess<String>(builder);
            Assert.Equal("Hi", result);
        }

        [Fact]
        public void RunProcessCommandMultiArgsAfter()
        {
            var runner = mockup.Get<IShellRunner<PowershellCoreRunnerTests>>();
            var factory = mockup.Get<IShellCommandBuilderFactory<PowershellCoreRunnerTests>>();
            var builder = factory.Create();
            var info = "Hi";
            var dummy = "Dummy";
            builder.AddCommand($"'Before'");
            builder.AddResultCommand($"{info}");
            builder.AddCommand($"{dummy}");
            builder.AddCommand($"{dummy}");
            builder.AddCommand($"{dummy}");
            builder.AddCommand($"'After'");
            var result = runner.RunProcess<String>(builder);
            Assert.Equal("Hi", result);
        }

        class HostInfo
        {
            public String Name { get; set; }
        }

        [Fact]
        public void GetHost()
        {
            var runner = mockup.Get<IShellRunner<PowershellCoreRunnerTests>>();
            var factory = mockup.Get<IShellCommandBuilderFactory<PowershellCoreRunnerTests>>();
            var builder = factory.Create();
            builder.JsonDepth = 2; //This command benefits from a small json depth
            builder.AddResultCommand($"Get-Host");
            var result = runner.RunProcess<HostInfo>(builder);
            Assert.Equal("ConsoleHost", result.Name);
        }

        [Fact]
        public void FailMultipleResults()
        {
            var runner = mockup.Get<IShellRunner<PowershellCoreRunnerTests>>();
            var factory = mockup.Get<IShellCommandBuilderFactory<PowershellCoreRunnerTests>>();
            var builder = factory.Create();
            builder.AddResultCommand($"'Hi'");
            Assert.Throws<InvalidOperationException>(() => builder.AddResultCommand($"'Hi'"));
        }

        [Fact]
        public void RunProcessVoid()
        {
            var runner = mockup.Get<IShellRunner<PowershellCoreRunnerTests>>();
            var numTimes = 5;
            runner.RunProcessVoid($"ping threax.com -n {numTimes}");
        }

        [Fact]
        public void RunProcessVoidEnumerable()
        {
            var runner = mockup.Get<IShellRunner<PowershellCoreRunnerTests>>();
            var numTimes = 5;
            runner.RunProcessVoid(new FormattableString[] { $"ping threax.com", $" -n {numTimes}" });
        }

        [Fact]
        public void RunProcessVoidFail()
        {
            var runner = mockup.Get<IShellRunner<PowershellCoreRunnerTests>>();
            Assert.Throws<InvalidOperationException>(() => runner.RunProcessVoid($"ping"));
        }

        [Fact]
        public void RunProcessJToken()
        {

            var runner = mockup.Get<IShellRunner<PowershellCoreRunnerTests>>();
            var name = "Test";
            var value = "SomeValue";
            dynamic result = runner.RunProcess($"[PSCustomObject]@{{ Name = {name}; Value = {value}; }} | ConvertTo-Json -Depth 2");
            Assert.Equal("Test", (string)result.Name);
        }

        [Fact]
        public void RunProcessJTokenFail()
        {

            var runner = mockup.Get<IShellRunner<PowershellCoreRunnerTests>>();
            var name = "Test";
            var value = "SomeValue";
            Assert.Throws<InvalidOperationException>(() => runner.RunProcess($"[PSCustomObject]@{{ Name = {name}; Value = {value}; }} | ConvertTo-Json -Depth 2; throw;"));
        }

        [Fact]
        public void RunProcessJTokenEnumerable()
        {

            var runner = mockup.Get<IShellRunner<PowershellCoreRunnerTests>>();
            var name = "Test";
            var value = "SomeValue";
            
            dynamic result = runner.RunProcess(
                new FormattableString[] { 
                    $"[PSCustomObject]@{{ Name = {name};", 
                    $" Value = {value}; }}", 
                    $" | ConvertTo-Json -Depth 2" 
                });

            Assert.Equal("Test", (string)result.Name);
            Assert.Equal("SomeValue", (string)result.Value);
        }

        class TestObj
        {
            public String Name { get; set; }

            public String Value { get; set; }
        }

        [Fact]
        public void RunProcessObject()
        {
            var runner = mockup.Get<IShellRunner<PowershellCoreRunnerTests>>();
            var name = "Test";
            var value = "SomeValue";
            var result = runner.RunProcess<TestObj>($"[PSCustomObject]@{{ Name = {name}; Value = {value}; }} | ConvertTo-Json -Depth 2");
            Assert.Equal("Test", result.Name);
        }

        [Fact]
        public void RunProcessObjectFail()
        {
            var runner = mockup.Get<IShellRunner<PowershellCoreRunnerTests>>();
            var name = "Test";
            var value = "SomeValue";
            Assert.Throws<InvalidOperationException>(() => runner.RunProcess<TestObj>($"[PSCustomObject]@{{ Name = {name}; Value = {value}; }} | ConvertTo-Json -Depth 2; throw;"));
        }

        [Fact]
        public void RunProcessObjectEnumerable()
        {
            var runner = mockup.Get<IShellRunner<PowershellCoreRunnerTests>>();
            var name = "Test";
            var value = "SomeValue";
            
            var result = runner.RunProcess<TestObj>(
                new FormattableString[] {
                    $"[PSCustomObject]@{{ Name = {name};", 
                    $" Value = {value}; }}", 
                    $" | ConvertTo-Json -Depth 2" 
                });

            Assert.Equal("Test", result.Name);
            Assert.Equal("SomeValue", result.Value);
        }
    }
}
