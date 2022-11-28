using System;
using System.Diagnostics;
using System.Text.Json;
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

        [Fact]
        public void JsonProcess()
        {
            var processRunner = new ProcessRunner();
            var jsonObj = new { hello = "hi" };
            var startInfo = new ProcessStartInfo("pwsh") { ArgumentList = { "-c", $"'{JsonSerializer.Serialize(jsonObj)}'" } };
            var result = processRunner.RunJsonProcess(startInfo, "could not parse json result");
            Assert.Equal("hi", result["hello"].ToString());
        }

        [Fact]
        public void JsonProcessFail()
        {
            var processRunner = new ProcessRunner();
            var startInfo = new ProcessStartInfo("pwsh") { ArgumentList = { "-c", $"'broken'" } };
            Assert.ThrowsAny<Exception>(() => processRunner.RunJsonProcess(startInfo, "could not parse json result"));
        }

        class TestType
        {
            public String Hello { get; set; } = "hi";
        }

        [Fact]
        public void JsonProcessTyped()
        {
            var processRunner = new ProcessRunner();
            var jsonObj = new TestType();
            var startInfo = new ProcessStartInfo("pwsh") { ArgumentList = { "-c", $"'{JsonSerializer.Serialize(jsonObj)}'" } };
            var result = processRunner.RunJsonProcess<TestType>(startInfo, "could not parse json result");
            Assert.Equal("hi", result.Hello);
        }

        [Fact]
        public void JsonProcessTypedFail()
        {
            var processRunner = new ProcessRunner();
            var jsonObj = new TestType();
            var startInfo = new ProcessStartInfo("pwsh") { ArgumentList = { "-c", $"'broken'" } };
            Assert.ThrowsAny<Exception>(() => processRunner.RunJsonProcess<TestType>(startInfo, "could not parse json result"));
        }

        [Fact]
        public void StringProcess()
        {
            var processRunner = new ProcessRunner();
            var startInfo = new ProcessStartInfo("pwsh") { ArgumentList = { "-c", "'hi'" } };
            var result = processRunner.RunStringProcess(startInfo, "could not run string process");
            Assert.Equal("hi", result);
        }

        [Fact]
        public void StringProcessFail()
        {
            var processRunner = new ProcessRunner();
            var startInfo = new ProcessStartInfo("pwsh") { ArgumentList = { "-thisisnotavalidarg", "'hi'" } };
            Assert.ThrowsAny<Exception>(() => processRunner.RunStringProcess(startInfo, "could not run string process"));
        }

        [Fact]
        public void StringProcessAddNewlines()
        {
            var processRunner = new ProcessRunner();
            var startInfo = new ProcessStartInfo("pwsh") { ArgumentList = { "-c", "'hi'" } };
            var result = processRunner.RunStringProcessAddNewlines(startInfo, "could not run string process");
            Assert.Equal("hi" + Environment.NewLine, result);
        }

        [Fact]
        public void StringProcessAddNewlinesFail()
        {
            var processRunner = new ProcessRunner();
            var startInfo = new ProcessStartInfo("pwsh") { ArgumentList = { "-thisisnotavalidarg", "'hi'" } };
            Assert.ThrowsAny<Exception>(() => processRunner.RunStringProcessAddNewlines(startInfo, "could not run string process"));
        }
    }
}
