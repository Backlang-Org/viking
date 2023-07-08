using System.ComponentModel;
using Spectre.Console;
using Spectre.Console.Cli;
using viking.Core;

namespace viking.Commands;

[Description("Run all tests")]
internal sealed class TestCommand : Command
{
    public override int Execute(CommandContext context)
    {
        DotnetWrapper.Test();
        
        return 0;
    }
}