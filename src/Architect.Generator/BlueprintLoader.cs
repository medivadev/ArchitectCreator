using System.Text.Json;
using Architect.Core.Models;

namespace Architect.Generator;

public sealed class BlueprintLoader
{
    public Blueprint Load(string path)
    {
        var json = File.ReadAllText(path);

        return JsonSerializer.Deserialize<Blueprint>(json)!
            ?? throw new InvalidOperationException("TTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTT");
    }
}