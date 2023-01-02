using System;
using System.Diagnostics;
using System.Text;
using System.Text.Json.Nodes;

namespace Threax.ProcessHelper;

public static class ProcessRunnerExtensions
{

    public static void RunVoid(this IProcessRunner processRunner, ProcessStartInfo startInfo, String errorMessage, int validExitCode = 0)
    {
        RunVoid(processRunner, startInfo, r => r != validExitCode ? errorMessage : null);
    }

    public static void RunVoid(this IProcessRunner processRunner, ProcessStartInfo startInfo, Func<int, string?> handleExitCode)
    {
        var exit = processRunner.Run(startInfo);

        var error = handleExitCode(exit);
        if (error != null)
        {
            throw new InvalidOperationException($"{error}. Got exit code '{exit}'.");
        }
    }

    public static JsonNode? RunJsonProcess(this IProcessRunner processRunner, ProcessStartInfo startInfo, String errorMessage, int validExitCode = 0)
    {
        return RunJsonProcess(processRunner, startInfo, r => r != validExitCode ? errorMessage : null);
    }

    public static JsonNode? RunJsonProcess(this IProcessRunner processRunner, ProcessStartInfo startInfo, Func<int, string?> handleExitCode)
    {
        var jsonRunner = new JsonOutputProcessRunner(processRunner, true);
        jsonRunner.Run(startInfo);

        var error = handleExitCode(jsonRunner.LastExitCode);
        if (error != null)
        {
            throw new InvalidOperationException($"{error}. Got exit code '{jsonRunner.LastExitCode}'.");
        }

        return jsonRunner.GetResult();
    }

    public static T? RunJsonProcess<T>(this IProcessRunner processRunner, ProcessStartInfo startInfo, String errorMessage, int validExitCode = 0)
    {
        return RunJsonProcess<T>(processRunner, startInfo, r => r != validExitCode ? errorMessage : null);
    }

    public static T? RunJsonProcess<T>(this IProcessRunner processRunner, ProcessStartInfo startInfo, Func<int, string?> handleExitCode)
    {
        var jsonRunner = new JsonOutputProcessRunner(processRunner, true);
        jsonRunner.Run(startInfo);

        var error = handleExitCode(jsonRunner.LastExitCode);
        if (error != null)
        {
            throw new InvalidOperationException($"{error}. Got exit code '{jsonRunner.LastExitCode}'.");
        }

        return jsonRunner.GetResult<T>();
    }

    public static String RunStringProcess(this IProcessRunner processRunner, ProcessStartInfo startInfo, String errorMessage, int validExitCode = 0)
    {
        return RunStringProcess(processRunner, startInfo, r => r != validExitCode ? errorMessage : null);
    }

    public static String RunStringProcess(this IProcessRunner processRunner, ProcessStartInfo startInfo, Func<int, string?> handleExitCode)
    {
        var sb = new StringBuilder();
        var exit = processRunner.Run(startInfo, new ProcessEvents()
        {
            OutputDataReceived = (o, i) =>
            {
                i.AllowOutput = false;
                sb.Append(i.DataReceivedEventArgs.Data);
            }
        });

        var error = handleExitCode(exit);
        if (error != null)
        {
            throw new InvalidOperationException($"{error}. Got exit code '{exit}'.");
        }

        return sb.ToString();
    }

    /// <summary>
    /// Run a string process using a stringbuilder to process each line of output.
    /// </summary>
    /// <returns></returns>
    public static String RunStringProcessAddNewlines(this IProcessRunner processRunner, ProcessStartInfo startInfo, String errorMessage, int validExitCode = 0)
    {
        return RunStringProcessAddNewlines(processRunner, startInfo, r => r != validExitCode ? errorMessage : null);
    }

    /// <summary>
    /// Run a string process using a stringbuilder to process each line of output.
    /// </summary>
    /// <returns></returns>
    public static String RunStringProcessAddNewlines(this IProcessRunner processRunner, ProcessStartInfo startInfo, Func<int, string?> handleExitCode)
    {
        var sb = new StringBuilder();
        var exit = processRunner.Run(startInfo, new ProcessEvents()
        {
            OutputDataReceived = (o, i) =>
            {
                i.AllowOutput = false;
                if (i.DataReceivedEventArgs.Data != null)
                {
                    sb.AppendLine(i.DataReceivedEventArgs.Data);
                }
            }
        });

        var error = handleExitCode(exit);
        if (error != null)
        {
            throw new InvalidOperationException($"{error}. Got exit code '{exit}'.");
        }

        return sb.ToString();
    }
}
