using System.ComponentModel;
using Spectre.Console;
using Spectre.Console.Cli;
using viking.Core;

namespace viking.Commands;

[Description("Build project and run it")]
internal sealed class RunCommand : Command
{
    public override int Execute(CommandContext context)
    {
        DotnetWrapper.Run();
        
        return 0;
    }
}