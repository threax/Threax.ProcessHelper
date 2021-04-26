using Microsoft.Extensions.DependencyInjection;
using System;
using Threax.AspNetCore.Tests;
using Xunit;

namespace Threax.ProcessHelper.Pwsh.Tests
{
    public class PowershellCoreRunnerDiTests
    {
        Mockup mockup = new Mockup();

        [Fact]
        public void CallArgumentBuilder()
        {
            bool called = false;
            mockup.MockServiceCollection.AddThreaxPwshProcessHelper<PowershellCoreRunnerTests>(o =>
            {
                var originalArgumentBuilder = o.CreateArgumentBuilder;
                o.CreateArgumentBuilder = s =>
                {
                    called = true;
                    return originalArgumentBuilder(s);
                };
            });

            var runner = mockup.Get<IPowershellCoreRunner<PowershellCoreRunnerTests>>();
            var commandBuilder = mockup.Get<IPwshCommandBuilder>();
            commandBuilder.AddResultCommand("'Hi'");
            var result = runner.RunCommand<string>(commandBuilder);
            Assert.Equal("Hi", result);
            Assert.True(called);
        }

        [Fact]
        public void CreateCustomType()
        {
            IPwshArgumentBuilder argBuilder = null;
            mockup.MockServiceCollection.AddThreaxPwshProcessHelper<PowershellCoreRunnerTests>(o =>
            {
                var originalArgumentBuilder = o.CreateArgumentBuilder;
                o.CreateArgumentBuilder = s =>
                {
                    argBuilder = s.GetRequiredService<IPwshArgumentBuilder<PowershellCoreRunnerDiTests>>();
                    return argBuilder;
                };
            });

            var runner = mockup.Get<IPowershellCoreRunner<PowershellCoreRunnerTests>>();
            var commandBuilder = mockup.Get<IPwshCommandBuilder>();
            commandBuilder.AddResultCommand("'Hi'");
            var result = runner.RunCommand<string>(commandBuilder);
            Assert.Equal("Hi", result);
            Assert.IsAssignableFrom<IPwshArgumentBuilder<PowershellCoreRunnerDiTests>>(argBuilder);
        }
    }
}
