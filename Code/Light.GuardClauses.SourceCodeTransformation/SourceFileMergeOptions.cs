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

    public string SourceFolder { get; init; } = string.Empty;

    public string TargetFile { get; init; } = string.Empty;

    public bool ChangePublicTypesToInternalTypes { get; init; } = true;

    public string BaseNamespace { get; init; } = "Light.GuardClauses";

    public bool RemoveContractAnnotations { get; init; } = false;

    public bool IncludeJetBrainsAnnotations { get; init; } = true;

    public bool IncludeJetBrainsAnnotationsUsing { get; init; } = true;

    public bool IncludeVersionComment { get; init; } = true;

    public bool RemoveOverloadsWithExceptionFactory { get; init; } = false;

    public bool IncludeCodeAnalysisNullableAttributes { get; init; } = true;

    public bool IncludeValidatedNotNullAttribute { get; init; } = true;

    public bool RemoveValidatedNotNull { get; init; } = false;

    public bool RemoveDoesNotReturn { get; init; } = false;

    public bool RemoveNotNullWhen { get; init; } = false;

    public bool IncludeCallerArgumentExpressionAttribute { get; init; } = true;

    public bool RemoveCallerArgumentExpressions { get; init; } = false;
}