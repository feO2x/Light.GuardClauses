using System;
using System.Collections.Generic;
using System.IO;

namespace Light.GuardClauses.SourceCodeTransformation;

internal static class RepositoryLayout
{
    private const string RepositoryMarkerFileName = "Light.GuardClauses.slnx";

    public static DirectoryInfo? TryFindRepositoryRoot(IEnumerable<string> searchRoots)
    {
        var searchedDirectories = new HashSet<string>(StringComparer.Ordinal);
        foreach (var searchRoot in searchRoots)
        {
            if (string.IsNullOrWhiteSpace(searchRoot))
            {
                continue;
            }

            var currentDirectory = new DirectoryInfo(Path.GetFullPath(searchRoot));
            if (!currentDirectory.Exists)
            {
                continue;
            }

            while (currentDirectory != null && searchedDirectories.Add(currentDirectory.FullName))
            {
                if (File.Exists(Path.Combine(currentDirectory.FullName, RepositoryMarkerFileName)))
                {
                    return currentDirectory;
                }

                currentDirectory = currentDirectory.Parent;
            }
        }

        return null;
    }
}
