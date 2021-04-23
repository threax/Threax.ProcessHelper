using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using Threax.AspNetCore.Tests;
using Xunit;

namespace Threax.ProcessHelper.Tests
{
    public class JsonProcessRunnerTests
    {
        Mockup mockup = new Mockup();

        public JsonProcessRunnerTests()
        {
            mockup.MockServiceCollection.AddThreaxProcessHelper<ProcessRunnerTests>();
        }

        [Fact]
        public void Echo()
        {
            var factory = mockup.Get<IProcessRunnerFactory<ProcessRunnerTests>>();
            var processRunner = new JsonOutputProcessRunner(factory.Create());
            var startInfo = new ProcessStartInfo("pwsh", $"-c '{JsonOutputProcessRunner.DefaultJsonStart}'; 'hi' | ConvertTo-Json; '{JsonOutputProcessRunner.DefaultJsonEnd}'");
            var result = processRunner.Run(startInfo);
            Assert.Equal(0, result);

            Assert.True(processRunner.HadJsonOutput);

            var objResult = processRunner.GetResult<String>();
            Assert.Equal("hi", objResult);
        }

        [Fact]
        public void Null()
        {
            var factory = mockup.Get<IProcessRunnerFactory<ProcessRunnerTests>>();
            var processRunner = new JsonOutputProcessRunner(factory.Create());
            var startInfo = new ProcessStartInfo("pwsh", $"-c '{JsonOutputProcessRunner.DefaultJsonStart}'; $null | ConvertTo-Json; '{JsonOutputProcessRunner.DefaultJsonEnd}'");
            var result = processRunner.Run(startInfo);
            Assert.Equal(0, result);

            Assert.True(processRunner.HadJsonOutput);

            var objResult = processRunner.GetResult<String>();
            Assert.Null(objResult);
        }

        [Fact]
        public void True()
        {
            var factory = mockup.Get<IProcessRunnerFactory<ProcessRunnerTests>>();
            var processRunner = new JsonOutputProcessRunner(factory.Create());
            var startInfo = new ProcessStartInfo("pwsh", $"-c '{JsonOutputProcessRunner.DefaultJsonStart}'; $true | ConvertTo-Json; '{JsonOutputProcessRunner.DefaultJsonEnd}'");
            var result = processRunner.Run(startInfo);
            Assert.Equal(0, result);

            Assert.True(processRunner.HadJsonOutput);

            var objResult = processRunner.GetResult<bool>();
            Assert.True(objResult);
        }

        [Fact]
        public void False()
        {
            var factory = mockup.Get<IProcessRunnerFactory<ProcessRunnerTests>>();
            var processRunner = new JsonOutputProcessRunner(factory.Create());
            var startInfo = new ProcessStartInfo("pwsh", $"-c '{JsonOutputProcessRunner.DefaultJsonStart}'; $false | ConvertTo-Json; '{JsonOutputProcessRunner.DefaultJsonEnd}'");
            var result = processRunner.Run(startInfo);
            Assert.Equal(0, result);

            Assert.True(processRunner.HadJsonOutput);

            var objResult = processRunner.GetResult<bool>();
            Assert.False(objResult);
        }

        [Fact]
        public void Number()
        {
            var factory = mockup.Get<IProcessRunnerFactory<ProcessRunnerTests>>();
            var processRunner = new JsonOutputProcessRunner(factory.Create());
            var startInfo = new ProcessStartInfo("pwsh", $"-c '{JsonOutputProcessRunner.DefaultJsonStart}'; 1 | ConvertTo-Json; '{JsonOutputProcessRunner.DefaultJsonEnd}'");
            var result = processRunner.Run(startInfo);
            Assert.Equal(0, result);

            Assert.True(processRunner.HadJsonOutput);

            var objResult = processRunner.GetResult<int>();
            Assert.Equal(1, objResult);
        }

        class TestClass
        {
            public int Number { get; set; }

            public String String { get; set; }
        }

        [Fact]
        public void Object()
        {
            var factory = mockup.Get<IProcessRunnerFactory<ProcessRunnerTests>>();
            var processRunner = new JsonOutputProcessRunner(factory.Create());
            var startInfo = new ProcessStartInfo("pwsh", $"-c '{JsonOutputProcessRunner.DefaultJsonStart}'; New-Object -TypeName PSObject -Property @{{Number = 4; String = 'hello';}} | ConvertTo-Json; '{JsonOutputProcessRunner.DefaultJsonEnd}'");
            var result = processRunner.Run(startInfo);
            Assert.Equal(0, result);

            Assert.True(processRunner.HadJsonOutput);

            var objResult = processRunner.GetResult<TestClass>();
            Assert.Equal(4, objResult.Number);
            Assert.Equal("hello", objResult.String);
        }

        [Fact]
        public void EchoError()
        {
            var factory = mockup.Get<IProcessRunnerFactory<ProcessRunnerTests>>();
            var processRunner = new JsonOutputProcessRunner(factory.Create());
            var startInfo = new ProcessStartInfo("pwsh", "-c Write-Error 'hi'");
            var result = processRunner.Run(startInfo);
            Assert.Equal(1, result);
            Assert.False(processRunner.HadJsonOutput);
        }

        [Fact]
        public void Fail()
        {
            var factory = mockup.Get<IProcessRunnerFactory<ProcessRunnerTests>>();
            var processRunner = new JsonOutputProcessRunner(factory.Create());
            var startInfo = new ProcessStartInfo("pwsh", "asdfdsaf");
            var result = processRunner.Run(startInfo);
            Assert.Equal(64, result);
            Assert.False(processRunner.HadJsonOutput);
        }
    }
}
