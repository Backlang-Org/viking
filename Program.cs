using Microsoft.Extensions.DependencyInjection;
using Spectre.Console;
using Spectre.Console.Cli;
using viking.Commands;

public static class Program
{
    public static int Main(string[] args)
    {
        var app = new CommandApp();
        app.Configure(_ =>
        {
            _.PropagateExceptions();
            _.ValidateExamples();
            _.SetApplicationName("viking");
            _.SetExceptionHandler(ex =>
            {
                AnsiConsole.WriteException(ex, ExceptionFormats.ShortenEverything);
            });

            AddCommands(_);
        });
        
        return app.Run(args);
    }

    private static void AddCommands(IConfigurator _)
    {
        _.AddCommand<AddCommand>("add");
        _.AddCommand<BuildCommand>("build");
        _.AddCommand<CleanCommand>("clean");
        _.AddCommand<NewCommand>("new");
        _.AddCommand<RunCommand>("run");
        _.AddCommand<TestCommand>("test");
        _.AddCommand<UpdateCommand>("update");
    }
}