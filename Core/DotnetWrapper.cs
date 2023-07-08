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

    public static void NewTemplate(string path)
    {
        File.WriteAllText( Path.Combine(path, "src", "Program.back"), @"func main() {
    print(""hello world"");
}");
        File.WriteAllText(Path.Combine(path, path + ".backproj"), @"<Project Sdk=""Backlang.NET.Sdk/1.0.84"">
                    <PropertyGroup>
                <OutputType>Exe</OutputType>
            <Description>TestConsole</Description>
            <TargetFramework>net7</TargetFramework>
            <AssemblyName>TestConsole</AssemblyName>
            <IncludeBuildOutput>true</IncludeBuildOutput>
            </PropertyGroup>
            </Project>");
    }
}