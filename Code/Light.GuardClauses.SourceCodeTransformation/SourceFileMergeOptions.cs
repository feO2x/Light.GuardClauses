using System.IO;

namespace Light.GuardClauses.SourceCodeTransformation;

public sealed record SourceFileMergeOptions
{
    public SourceFileMergeOptions()
    {
        try
        {
            var currentDirectory = new DirectoryInfo(".");
            while (currentDirectory != null && currentDirectory.Name != "Code")
                currentDirectory = currentDirectory.Parent;

            if (currentDirectory == null)
                return;

            TargetFile = Path.Combine(currentDirectory.FullName, "Light.GuardClauses.Source", "Light.GuardClauses.cs");
            SourceFolder = Path.Combine(currentDirectory.FullName, "Light.GuardClauses");
        }
        // ReSharper disable once EmptyGeneralCatchClause
        catch { }
    }

    public string SourceFolder { get; set; } = string.Empty;

    public string TargetFile { get; set; } = string.Empty;

    public bool ChangePublicTypesToInternalTypes { get; set; } = true;

    public string BaseNamespace { get; set; } = "Light.GuardClauses";

    public bool RemoveContractAnnotations { get; set; } = false;

    public bool IncludeJetBrainsAnnotations { get; set; } = true;

    public bool IncludeJetBrainsAnnotationsUsing { get; set; } = true;

    public bool IncludeVersionComment { get; set; } = true;

    public bool RemoveOverloadsWithExceptionFactory { get; set; } = false;

    public bool IncludeCodeAnalysisNullableAttributes { get; set; } = true;

    public bool IncludeCallerArgumentExpressionAttribute { get; set; } = true;
}