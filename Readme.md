# Threax.ProcessHelper
A wrapper library for Process.Start that makes writing integration with other processes easier and more secure. All commands use FormattableStrings to allow for secure data exchange using environment variables. Results are interpreted as json, which includes plain string support for processes that do not output json. Currently there is only a runner for powershell core, but other shell support is planned. How cross platform your implementations are depends on how you write them.

## Writing commands
Most commands can be written like the following:
```
var context, dockerFile, tag;
shellRunner.RunProcessVoid($"docker build {context} -f {dockerFile} -t {tag}");
```
This will take the values of the variables context, dockerFile and tag and pass them to the real shell command as environment variables. As a result no special formatting is needed on any arguments like quotes and no escaping needs to be done. In fact it is discoraged to make the variables process on the shell side, since that may lead them to execute and allow for shell injection. If the variables are passed as is any bad shell code trying to be injected will be nullified by the fact that it is in a variable. The variables are set using ProcessStartInfo's environment variable collection, they are never interpreted by the shell itself.

Otherwise there are a few different methods for calling shell commands and getting json output interpreted as a c# type or a Newtonsoft.Json JToken, which can be used with dynamic.