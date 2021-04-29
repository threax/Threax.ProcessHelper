using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

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

        public int Run(ProcessStartInfo startInfo, ProcessEvents? events = null)
        {
            jsonBuilder.Clear();
            HadJsonOutput = false;
            bool readJsonLines = false;
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
                    if (!String.IsNullOrEmpty(e.Data))
                    {
                        if (e.Data == JsonStart)
                        {
                            readJsonLines = true;
                        }
                        else if (e.Data == JsonEnd)
                        {
                            readJsonLines = false;
                        }
                        else if (readJsonLines) //Else is intentional, want to skip start and end lines.
                        {
                            //Skip any lines that start with text in the skip lines collection
                            if(!StartWithSkipLines.Any(i => e.Data.StartsWith(i)))
                            {
                                HadJsonOutput = true;
                                jsonBuilder.AppendLine(e.Data);
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
            return GetResult().ToObject<T>();
        }

        /// <summary>
        /// Get the result as a JToken. You will always get a JToken, but it might represent null.
        /// </summary>
        /// <returns>A JToken of the parsed returned json.</returns>
        public JToken GetResult()
        {
            JToken result;
            if (HadJsonOutput)
            {
                var json = jsonBuilder.ToString();
                result = JToken.Parse(json); 
            }
            else
            {
                result = JToken.Parse("null");
            }

            return result;
        }

        /// <summary>
        /// This will be true if there was any json output.
        /// </summary>
        public bool HadJsonOutput { get; private set; }

        /// <summary>
        /// The last exit code produced by this runner.
        /// </summary>
        public int LastExitCode { get; set; }
    }
}
