namespace Architect.Core;

public interface IProjectGenerator
{
    Task GenerateAsync(
        string projectName,
        CancellationToken cancellationToken = default);
}