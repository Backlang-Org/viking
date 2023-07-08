using System.ComponentModel;
using Spectre.Console;
using Spectre.Console.Cli;
using viking.Core;

namespace viking.Commands;

[Description("Clean the temporary files")]
internal sealed class CleanCommand : Command
{
    public override int Execute(CommandContext context)
    {
        DotnetWrapper.Clean();
        
        return 0;
    }
}