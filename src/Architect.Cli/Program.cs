using System.CommandLine;

using Architect.Core;
using Architect.Generator;

var rootCommand = new RootCommand(
    "Architect Creator");

var projectNameArgument = new Argument<string>(
    name: "projectName");

var newCommand = new Command(
    name: "new",
    description: "Create a new project");

var versionCommand = new Command(
    name: "version",
    description: "Show project version");

newCommand.Arguments.Add(projectNameArgument);

newCommand.SetAction(async parseResult =>
{
    var projectName =
        parseResult.GetValue(projectNameArgument);

    Console.WriteLine(
        $"Creating project: {projectName}");

    IProcessRunner processRunner =
        new ProcessRunner();

    IProjectGenerator generator =
        new CleanArchitectureGenerator(processRunner);
    
    await generator.GenerateAsync(projectName!);
});

IProcessRunner processRunner =
        new ProcessRunner();

    IProjectGenerator generator =
        new CleanArchitectureGenerator(processRunner);
await generator.GenerateAsync("EEE");

versionCommand.SetAction(_ =>
{
    Console.WriteLine(
        $"Architect creator v0.1.0");
});

rootCommand.Subcommands.Add(newCommand);
rootCommand.Subcommands.Add(versionCommand);

return await rootCommand.Parse(args).InvokeAsync();