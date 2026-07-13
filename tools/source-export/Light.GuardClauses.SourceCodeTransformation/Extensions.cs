using System.IO;

namespace Light.GuardClauses.SourceCodeTransformation;

public static class Extensions
{
    public static string ReadContent(this FileInfo fileInfo) => File.ReadAllText(fileInfo.FullName);
}