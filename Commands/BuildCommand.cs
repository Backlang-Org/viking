using System.ComponentModel;
using Spectre.Console;
using Spectre.Console.Cli;
using viking.Core;

namespace viking.Commands;

[Description("Build executable from source code")]
internal sealed class BuildCommand : Command
{
    public override int Execute(CommandContext context)
    {
        DotnetWrapper.Build();
        
        return 0;
    }
}