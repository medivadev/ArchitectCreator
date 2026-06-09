namespace Architect.Core;

public interface IProcessRunner
{
    Task<int> RunAsync(
        string fileName,
        string arguments,
        string workingDirectory,
        CancellationToken cancellationToken = default);
}