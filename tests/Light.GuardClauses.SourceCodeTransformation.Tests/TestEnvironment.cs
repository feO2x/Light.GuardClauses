using System;
using System.IO;

namespace Light.GuardClauses.SourceCodeTransformation.Tests;

internal static class TestEnvironment
{
    private const string RepositoryMarkerFileName = "Light.GuardClauses.sln";

    public static DirectoryInfo RepositoryRoot { get; } = FindRepositoryRoot();

    public static DirectoryInfo SourceDirectory { get; } =
        new (Path.Combine(RepositoryRoot.FullName, "src", "Light.GuardClauses"));

    public static DirectoryInfo FindRepositoryRoot()
    {
        var searchRoots = new[] { Directory.GetCurrentDirectory(), AppContext.BaseDirectory };
        foreach (var searchRoot in searchRoots)
        {
            var currentDirectory = new DirectoryInfo(searchRoot);
            while (currentDirectory != null)
            {
                if (File.Exists(Path.Combine(currentDirectory.FullName, RepositoryMarkerFileName)))
                {
                    return currentDirectory;
                }

                currentDirectory = currentDirectory.Parent;
            }
        }

        throw new InvalidOperationException(
            $"Could not locate the repository root using \"{RepositoryMarkerFileName}\"."
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
