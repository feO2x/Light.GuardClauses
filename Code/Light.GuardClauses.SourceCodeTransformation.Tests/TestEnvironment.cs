using System;
using System.IO;

namespace Light.GuardClauses.SourceCodeTransformation.Tests;

internal static class TestEnvironment
{
    public static DirectoryInfo CodeDirectory { get; } = FindCodeDirectory();

    public static DirectoryInfo SourceDirectory { get; } =
        new (Path.Combine(CodeDirectory.FullName, "Light.GuardClauses"));

    public static DirectoryInfo FindCodeDirectory()
    {
        var currentDirectory = new DirectoryInfo(".");
        do
        {
            if (currentDirectory.Name == "Code")
            {
                return currentDirectory;
            }

            currentDirectory = currentDirectory.Parent;
        } while (currentDirectory != null);

        throw new InvalidOperationException(
            "This test project does not reside in a folder called \"Code\" (directly or indirectly)."
        );
    }
}

internal sealed class TemporaryDirectory : IDisposable
{
    public TemporaryDirectory()
    {
        DirectoryPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(DirectoryPath);
    }

    public string DirectoryPath { get; }

    public void Dispose()
    {
        if (Directory.Exists(DirectoryPath))
        {
            Directory.Delete(DirectoryPath, true);
        }
    }
}
