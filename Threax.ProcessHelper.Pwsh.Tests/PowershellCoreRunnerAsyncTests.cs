using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;
using Threax.AspNetCore.Tests;
using Xunit;

namespace Threax.ProcessHelper.Pwsh.Tests
{
    public class PowershellCoreRunnerAsyncTests
    {
        Mockup mockup = new Mockup();

        public PowershellCoreRunnerAsyncTests()
        {
            mockup.MockServiceCollection.AddLogging();
            mockup.MockServiceCollection.AddThreaxPwshShellRunner();
        }

        [Fact]
        public async Task RunProcessCommandsVoidSecurityCheck()
        {
            var runner = mockup.Get<IShellRunner>();
            var builder = mockup.Get<IShellCommandBuilder>();
            var evil = "exit 44;";
            builder.AddCommand($"'Before'");
            builder.AddResultCommand($"{evil}");
            builder.AddCommand($"'After'");
            await runner.RunProcessVoidAsync(builder);
        }

        [Fact]
        public async Task RunProcessCommandsVoid()
        {
            var runner = mockup.Get<IShellRunner>();
            var builder = mockup.Get<IShellCommandBuilder>();
            builder.AddCommand($"'Before'");
            builder.AddResultCommand($"'Hi'");
            builder.AddCommand($"'After'");
            await runner.RunProcessVoidAsync(builder);
        }

        [Fact]
        public async Task RunProcessCommandsJTokenSecurityCheck()
        {
            var runner = mockup.Get<IShellRunner>();
            var builder = mockup.Get<IShellCommandBuilder>();
            var evil = "echo hi";
            builder.AddCommand($"'Before'");
            builder.AddResultCommand($"{evil}");
            builder.AddCommand($"'After'");
            var result = await runner.RunProcessAsync(builder);
            Assert.Equal("echo hi", result.ToString());
        }

        [Fact]
        public async Task RunProcessCommandsJToken()
        {
            var runner = mockup.Get<IShellRunner>();
            var builder = mockup.Get<IShellCommandBuilder>();
            builder.AddCommand($"'Before'");
            builder.AddResultCommand($"'Hi'");
            builder.AddCommand($"'After'");
            var result = await runner.RunProcessAsync(builder);
            Assert.Equal("Hi", result.ToString());
        }

        [Fact]
        public async Task RunProcessCommandJTokenArgs()
        {
            var runner = mockup.Get<IShellRunner>();
            var builder = mockup.Get<IShellCommandBuilder>();
            var info = "Hi";
            builder.AddCommand($"'Before'");
            builder.AddResultCommand($"{info}");
            builder.AddCommand($"'After'");
            var result = await runner.RunProcessAsync(builder);
            Assert.Equal("Hi", result.ToString());
        }

        [Fact]
        public async Task RunProcessCommandsObjectSecurityCheck()
        {
            var runner = mockup.Get<IShellRunner>();
            var builder = mockup.Get<IShellCommandBuilder>();
            var evil = "echo hi";
            builder.AddCommand($"'Before'");
            builder.AddResultCommand($"{evil}");
            builder.AddCommand($"'After'");
            var result = await runner.RunProcessAsync<String>(builder);
            Assert.Equal("echo hi", result);
        }

        [Fact]
        public async Task RunProcessCommandsObjectSecurityCheckRawProcessStringFail()
        {
            var runner = mockup.Get<IShellRunner>();
            var builder = mockup.Get<IShellCommandBuilder>();
            var evil = "echo hi";
            builder.AddCommand($"'Before'");
            builder.AddResultCommand($"{new RawProcessString(evil)}");
            builder.AddCommand($"'After'");
            var result = await runner.RunProcessAsync<String>(builder);
            Assert.NotEqual("echo hi", result);
            Assert.Equal("hi", result); //Since raw process string was used the command can execute and we get back "hi" not "echo hi" as we would not using RawProcessString.
        }

        [Fact]
        public async Task RunProcessCommandsObject()
        {
            var runner = mockup.Get<IShellRunner>();
            var builder = mockup.Get<IShellCommandBuilder>();
            builder.AddCommand($"'Before'");
            builder.AddResultCommand($"'Hi'");
            builder.AddCommand($"'After'");
            var result = await runner.RunProcessAsync<String>(builder);
            Assert.Equal("Hi", result);
        }

        [Fact]
        public async Task RunProcessCommandArgs()
        {
            var runner = mockup.Get<IShellRunner>();
            var builder = mockup.Get<IShellCommandBuilder>();
            var info = "Hi";
            builder.AddCommand($"'Before'");
            builder.AddResultCommand($"{info}");
            builder.AddCommand($"'After'");
            var result = await runner.RunProcessAsync<String>(builder);
            Assert.Equal("Hi", result);
        }

        [Fact]
        public async Task RunProcessCommandMultiArgsBefore()
        {
            var runner = mockup.Get<IShellRunner>();
            var builder = mockup.Get<IShellCommandBuilder>();
            var info = "Hi";
            var dummy = "Dummy";
            builder.AddCommand($"'Before'");
            builder.AddCommand($"{dummy}");
            builder.AddCommand($"{dummy}");
            builder.AddCommand($"{dummy}");
            builder.AddResultCommand($"{info}");
            builder.AddCommand($"'After'");
            var result = await runner.RunProcessAsync<String>(builder);
            Assert.Equal("Hi", result);
        }

        [Fact]
        public async Task RunProcessCommandMultiArgsAfter()
        {
            var runner = mockup.Get<IShellRunner>();
            var builder = mockup.Get<IShellCommandBuilder>();
            var info = "Hi";
            var dummy = "Dummy";
            builder.AddCommand($"'Before'");
            builder.AddResultCommand($"{info}");
            builder.AddCommand($"{dummy}");
            builder.AddCommand($"{dummy}");
            builder.AddCommand($"{dummy}");
            builder.AddCommand($"'After'");
            var result = await runner.RunProcessAsync<String>(builder);
            Assert.Equal("Hi", result);
        }

        class HostInfo
        {
            public String Name { get; set; }
        }

        [Fact]
        public async Task GetHost()
        {
            var runner = mockup.Get<IShellRunner>();
            var builder = mockup.Get<IShellCommandBuilder>();
            builder.JsonDepth = 2; //This command benefits from a small json depth
            builder.AddResultCommand($"Get-Host");
            var result = await runner.RunProcessAsync<HostInfo>(builder);
            Assert.Equal("ConsoleHost", result.Name);
        }

        [Fact]
        public async Task FailMultipleResults()
        {
            var runner = mockup.Get<IShellRunner>();
            var commandBuilder = mockup.Get<IShellCommandBuilder>();
            commandBuilder.AddResultCommand($"'Hi'");
            Assert.Throws<InvalidOperationException>(() => commandBuilder.AddResultCommand($"'Hi'"));
        }

        [Fact]
        public async Task RunProcessVoid()
        {
            var runner = mockup.Get<IShellRunner>();
            var numTimes = 5;
            await runner.RunProcessVoidAsync($"ping threax.com -n {numTimes}");
        }

        [Fact]
        public async Task RunProcessVoidEnumerable()
        {
            var runner = mockup.Get<IShellRunner>();
            var numTimes = 5;
            await runner.RunProcessVoidAsync(new FormattableString[] { $"ping threax.com", $" -n {numTimes}" });
        }

        [Fact]
        public async Task RunProcessVoidFail()
        {
            var runner = mockup.Get<IShellRunner>();
            await Assert.ThrowsAsync<InvalidOperationException>(() => runner.RunProcessVoidAsync($"ping"));
        }

        [Fact]
        public async Task RunProcessJToken()
        {

            var runner = mockup.Get<IShellRunner>();
            var name = "Test";
            var value = "SomeValue";
            dynamic result = await runner.RunProcessAsync($"[PSCustomObject]@{{ Name = {name}; Value = {value}; }} | ConvertTo-Json -Depth 2");
            Assert.Equal("Test", (string)result.Name);
        }

        [Fact]
        public async Task RunProcessJTokenFail()
        {

            var runner = mockup.Get<IShellRunner>();
            var name = "Test";
            var value = "SomeValue";
            await Assert.ThrowsAsync<InvalidOperationException>(() => runner.RunProcessAsync($"[PSCustomObject]@{{ Name = {name}; Value = {value}; }} | ConvertTo-Json -Depth 2; throw;"));
        }

        [Fact]
        public async Task RunProcessJTokenEnumerable()
        {

            var runner = mockup.Get<IShellRunner>();
            var name = "Test";
            var value = "SomeValue";
            
            dynamic result = await runner.RunProcessAsync(
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
        public async Task RunProcessJTokenNoOutput()
        {
            var runner = mockup.Get<IShellRunner>();
            var result = await runner.RunProcessAsync($"");
            Assert.Equal(JTokenType.Null, result.Type);
        }

        [Fact]
        public async Task RunProcessObject()
        {
            var runner = mockup.Get<IShellRunner>();
            var name = "Test";
            var value = "SomeValue";
            var result = await runner.RunProcessAsync<TestObj>($"[PSCustomObject]@{{ Name = {name}; Value = {value}; }} | ConvertTo-Json -Depth 2");
            Assert.Equal("Test", result.Name);
        }

        [Fact]
        public async Task RunProcessObjectFail()
        {
            var runner = mockup.Get<IShellRunner>();
            var name = "Test";
            var value = "SomeValue";
            await Assert.ThrowsAsync<InvalidOperationException>(() => runner.RunProcessAsync<TestObj>($"[PSCustomObject]@{{ Name = {name}; Value = {value}; }} | ConvertTo-Json -Depth 2; throw;"));
        }

        [Fact]
        public async Task RunProcessObjectEnumerable()
        {
            var runner = mockup.Get<IShellRunner>();
            var name = "Test";
            var value = "SomeValue";
            
            var result = await runner.RunProcessAsync<TestObj>(
                new FormattableString[] {
                    $"[PSCustomObject]@{{ Name = {name};", 
                    $" Value = {value}; }}", 
                    $" | ConvertTo-Json -Depth 2" 
                });

            Assert.Equal("Test", result.Name);
            Assert.Equal("SomeValue", result.Value);
        }

        [Fact]
        public async Task RunProcessObjectNoOutput()
        {
            var runner = mockup.Get<IShellRunner>();
            var result = await runner.RunProcessAsync<TestObj>($"");
            Assert.Null(result);
        }
    }
}
