using System;
using System.IO;

namespace Light.GuardClauses.SourceCodeTransformation;

public sealed record SourceFileMergeOptions
{
    public SourceFileMergeOptions()
    {
        try
        {
            var repositoryRoot = RepositoryLayout.TryFindRepositoryRoot(
                [Directory.GetCurrentDirectory(), AppContext.BaseDirectory]
            );
            if (repositoryRoot == null)
            {
                return;
            }

            TargetFile = Path.Combine(repositoryRoot.FullName, "Light.GuardClauses.SingleFile.cs");
            SourceFolder = Path.Combine(repositoryRoot.FullName, "src", "Light.GuardClauses");
        }
        // ReSharper disable once EmptyGeneralCatchClause
        catch { }
    }

    public string SourceFolder { get; init; } = string.Empty;

    public string TargetFile { get; init; } = string.Empty;

    public SourceTargetFramework TargetFramework { get; init; } = SourceTargetFramework.NetStandard2_0;

    public AssertionWhitelist AssertionWhitelist { get; init; } = new ();

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
