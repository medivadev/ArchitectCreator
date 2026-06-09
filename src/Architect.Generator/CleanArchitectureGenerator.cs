using Architect.Core;

namespace Architect.Generator;

public sealed class CleanArchitectureGenerator(
    IProcessRunner processRunner)
        : IProjectGenerator
{
    private readonly IProcessRunner _processRunner = processRunner;

    public async Task GenerateAsync(
        string projectName,
        CancellationToken cancellationToken = default)
    {
        var root = Path.Combine(
            Directory.GetCurrentDirectory(),
            projectName);
        
        if (Directory.Exists(root))
        {
            throw new InvalidOperationException(
                $"Project '{projectName}' already exists.");
        }

        // Create Root Directory

        Directory.CreateDirectory(root);

        // Generate sln or slnx

        await _processRunner.RunAsync(
            "dotnet",
            $"new sln -n {projectName}",
            root,
            cancellationToken);

        // Solution Directories

        var srcPath = Path.Combine(root, "src");
        Directory.CreateDirectory(srcPath);

        var testsPath = Path.Combine(root, "tests");
        Directory.CreateDirectory(testsPath);

        // Load Clean Architecture

        var blueprint = new BlueprintLoader().Load("templates/clean-architecture/blueprint.json");

        // Generate src projects

        foreach (var project in blueprint.Projects)
        {
            var projectName1 = 
                project.Name.Replace(
                    "{ProjectName}",
                    projectName);
            
            await _processRunner.RunAsync(
                "dotnet",
                $"new {project.Template} -n {projectName1}",
                srcPath,
                cancellationToken);
        }

        // await _processRunner.RunAsync(
        //     "dotnet",
        //     $"new webapi -n {projectName}.Api",
        //     srcPath,
        //     cancellationToken);

        // await _processRunner.RunAsync(
        //     "dotnet",
        //     $"new classlib -n {projectName}.Application",
        //     srcPath,
        //     cancellationToken);

        // await _processRunner.RunAsync(
        //     "dotnet",
        //     $"new classlib -n {projectName}.Domain",
        //     srcPath,
        //     cancellationToken);

        // await _processRunner.RunAsync(
        //     "dotnet",
        //     $"new classlib -n {projectName}.Infrastructure",
        //     srcPath,
        //     cancellationToken);

        
        // Test projects

        await _processRunner.RunAsync(
            "dotnet",
            $"new xunit -n {projectName}.UnitTests",
            testsPath,
            cancellationToken);

        
        // Register projects on sln

        await _processRunner.RunAsync(
            "dotnet",
            $"sln add src/{projectName}.Api/{projectName}.Api.csproj",
            root);
        
        await _processRunner.RunAsync(
            "dotnet",
            $"sln add src/{projectName}.Application/{projectName}.Application.csproj",
            root);

        await _processRunner.RunAsync(
            "dotnet",
            $"sln add src/{projectName}.Domain/{projectName}.Domain.csproj",
            root);

        await _processRunner.RunAsync(
            "dotnet",
            $"sln add src/{projectName}.Infrastructure/{projectName}.Infrastructure.csproj",
            root);

        // Add project references

        await _processRunner.RunAsync(
            "dotnet",
            $"add src/{projectName}.Application/{projectName}.Application.csproj reference src/{projectName}.Domain/{projectName}.Domain.csproj",
            root);

        await _processRunner.RunAsync(
            "dotnet",
            $"add src/{projectName}.Infrastructure/{projectName}.Infrastructure.csproj reference src/{projectName}.Application/{projectName}.Application.csproj",
            root);

        await _processRunner.RunAsync(
            "dotnet",
            $"add src/{projectName}.Infrastructure/{projectName}.Infrastructure.csproj reference src/{projectName}.Domain/{projectName}.Domain.csproj",
            root);

        await _processRunner.RunAsync(
            "dotnet",
            $"add src/{projectName}.Api/{projectName}.Api.csproj reference src/{projectName}.Application/{projectName}.Application.csproj",
            root);

        await _processRunner.RunAsync(
            "dotnet",
            $"add src/{projectName}.Api/{projectName}.Api.csproj reference src/{projectName}.Infrastructure/{projectName}.Infrastructure.csproj",
            root);

        await _processRunner.RunAsync(
            "dotnet",
            $"add src/{projectName}.UnitTests/{projectName}.UnitTests.csproj reference src/{projectName}.Application/{projectName}.Application.csproj",
            root);
    }
}