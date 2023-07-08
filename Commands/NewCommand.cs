using System.ComponentModel;
using GitSharp;
using Spectre.Console;
using Spectre.Console.Cli;
using Index = GitSharp.Index;

namespace viking.Commands;

[Description("Create new backlang project")]
internal sealed class NewCommand : Command<NewCommand.Settings>
{
    internal class Settings : CommandSettings
    {
        [CommandArgument(0, "<name>")]
        public string Name { get; set; }
    }
    
    public override int Execute(CommandContext context, Settings settings)
    {
        Directory.CreateDirectory(settings.Name);
        Directory.CreateDirectory(Path.Combine(settings.Name, "src"));

        var repository = Repository.Init(settings.Name);
        new Index(repository).AddAll();
        
        repository.Commit("Initial Commit");

        return 0;
    }
}