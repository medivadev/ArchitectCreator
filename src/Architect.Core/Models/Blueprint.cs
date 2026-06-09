namespace Architect.Core.Models;

public sealed class Blueprint
{
    public string SolutionTemplate { get; set; } = string.Empty;

    public List<ProjectDefinition> Projects { get; set; } = [];
}