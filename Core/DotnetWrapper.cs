using System.Text;
using CliWrap;

namespace viking.Core;

public static class DotnetWrapper
{
    private static void Invoke(string arguments)
    {
        var stdOutBuffer = new StringBuilder();
        var stdErrBuffer = new StringBuilder();

        Cli.Wrap("dotnet")
            .WithStandardOutputPipe(PipeTarget.ToStringBuilder(stdOutBuffer))
            .WithStandardErrorPipe(PipeTarget.ToStringBuilder(stdErrBuffer))
            .WithWorkingDirectory(Environment.CurrentDirectory)
            .WithValidation(CommandResultValidation.None)
            .WithArguments(arguments)
            .ExecuteAsync()
            .GetAwaiter()
            .GetResult();

        Console.Error.WriteLine(stdErrBuffer.ToString());
        Console.WriteLine(stdOutBuffer.ToString());
    }


    public static void Clean()
    {
        Invoke("clean");
    }

    public static void Build()
    {
        Invoke("build");
    }

    public static void Run()
    {
        Invoke("run");
    }

    public static void Test()
    {
        Invoke("test");
    }
}