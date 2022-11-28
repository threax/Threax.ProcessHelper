using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Threax.ProcessHelper
{
    public class JsonOutputProcessRunner : IProcessRunner
    {
        public const String DefaultJsonStart = "----Threax.ProcessHelper JSON Output Start----";
        public const String DefaultJsonEnd = "----Threax.ProcessHelper JSON Output End----";

        public String JsonStart { get; set; } = DefaultJsonStart;
        public String JsonEnd { get; set; } = DefaultJsonEnd;

        /// <summary>
        /// Skip any lines that start with the given text when reading json output.
        /// </summary>
        public List<String> StartWithSkipLines { get; } = new List<string>();

        private readonly IProcessRunner child;
        private readonly StringBuilder jsonBuilder = new StringBuilder();

        public JsonOutputProcessRunner(IProcessRunner child)
        {
            this.child = child;
        }

        public JsonOutputProcessRunner(IProcessRunner child, bool readAllLinesAsJson)
            :this(child)
        {
            this.ReadAllLinesAsJson = readAllLinesAsJson;
        }

        public int Run(ProcessStartInfo startInfo, ProcessEvents? events = null)
        {
            jsonBuilder.Clear();
            HadJsonOutput = false;
            bool readJsonLines = ReadAllLinesAsJson;
            LastExitCode = child.Run(startInfo, new ProcessEvents()
            {
                ProcessCreated = events?.ProcessCreated,
                ProcessCompleted = events?.ProcessCompleted,
                ErrorDataReceived = (s, e) =>
                {
                    events?.ErrorDataReceived?.Invoke(s, e);
                },
                OutputDataReceived = (s, e) =>
                {
                    if (!String.IsNullOrEmpty(e.DataReceivedEventArgs.Data))
                    {
                        if (e.DataReceivedEventArgs.Data == JsonStart)
                        {
                            readJsonLines = true;
                            e.AllowOutput = false;
                        }
                        else if (e.DataReceivedEventArgs.Data == JsonEnd)
                        {
                            readJsonLines = false;
                            e.AllowOutput = false;
                        }
                        else if (readJsonLines) //Else is intentional, want to skip start and end lines.
                        {
                            //Skip any lines that start with text in the skip lines collection
                            if(!StartWithSkipLines.Any(i => e.DataReceivedEventArgs.Data.StartsWith(i)))
                            {
                                HadJsonOutput = true;
                                jsonBuilder.AppendLine(e.DataReceivedEventArgs.Data);
                                e.AllowOutput = false;
                            }
                        }
                    }

                    events?.OutputDataReceived?.Invoke(s, e);
                }
            });

            return LastExitCode;
        }

        /// <summary>
        /// Parse the json result into T. Note that the returned T can be null.
        /// </summary>
        /// <typeparam name="T">The type to parse from the results.</typeparam>
        /// <returns>A T, which can be null.</returns>
        public T? GetResult<T>()
        {
            T? result;
            if (HadJsonOutput)
            {
                var json = jsonBuilder.ToString();
                result = JsonSerializer.Deserialize<T>(json);
            }
            else
            {
                result = default(T);
            }

            return result;
        }

        /// <summary>
        /// Get the result as a JsonNode. This will be null if the result was null.
        /// </summary>
        /// <returns>A JsonNode of the parsed returned json.</returns>
        public JsonNode? GetResult()
        {
            JsonNode? result;
            if (HadJsonOutput)
            {
                var json = jsonBuilder.ToString();
                result = JsonNode.Parse(json);
            }
            else
            {
                result = null;
            }

            return result;
        }

        /// <summary>
        /// Parse the json result into T. Note that the returned T can be null.
        /// </summary>
        /// <typeparam name="T">The type to parse from the results.</typeparam>
        /// <returns>A T, which can be null.</returns>
        public T? GetResult<T>(String errorMessage, int validExitCode = 0)
        {
            if (LastExitCode != validExitCode)
            {
                throw new InvalidOperationException($"Invalid exit code: '{LastExitCode}' expected '{validExitCode}'. Message: {errorMessage}");
            }

            return GetResult<T>();
        }

        /// <summary>
        /// Get the result as a JsonNode. This will be null if the result was null.
        /// </summary>
        /// <returns>A JsonNode of the parsed returned json.</returns>
        public JsonNode? GetResult(String errorMessage, int validExitCode = 0)
        {
            if(LastExitCode != validExitCode)
            {
                throw new InvalidOperationException($"Invalid exit code: '{LastExitCode}' expected '{validExitCode}'. Message: {errorMessage}");
            }

            return GetResult();
        }

        /// <summary>
        /// This will be true if there was any json output.
        /// </summary>
        public bool HadJsonOutput { get; private set; }

        /// <summary>
        /// Set this to true to read all lines as json.
        /// </summary>
        public bool ReadAllLinesAsJson { get; set; }

        /// <summary>
        /// The last exit code produced by this runner.
        /// </summary>
        public int LastExitCode { get; set; }
    }
}
