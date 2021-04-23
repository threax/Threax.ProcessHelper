using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Threax.ProcessHelper
{
    public class JsonOutputProcessRunner : IProcessRunner
    {
        public const String DefaultJsonStart = "----Threax.ProcessHelper JSON Output Start----";
        public const String DefaultJsonEnd = "----Threax.ProcessHelper JSON Output End----";

        public String JsonStart { get; set; } = DefaultJsonStart;
        public String JsonEnd { get; set; } = DefaultJsonEnd;

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
            return child.Run(startInfo, new ProcessEvents()
            {
                ProcessCreated = events?.ProcessCreated,
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
                            HadJsonOutput = true;
                            jsonBuilder.AppendLine(e.Data);
                        }
                    }

                    events?.OutputDataReceived?.Invoke(s, e);
                }
            });
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
            if (!HadJsonOutput)
            {
                throw new InvalidOperationException("Cannot get a result since no json output was provided by the underlying process.");
            }
            var json = jsonBuilder.ToString();
            return JToken.Parse(json);
        }

        /// <summary>
        /// This will be true if there was any json output.
        /// </summary>
        public bool HadJsonOutput { get; private set; }
    }
}
