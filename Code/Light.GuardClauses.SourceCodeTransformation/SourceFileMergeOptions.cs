using System.IO;

namespace Light.GuardClauses.SourceCodeTransformation
{
    public sealed class SourceFileMergeOptions
    {
        public SourceFileMergeOptions(string sourceFolder,
                                      string targetFile,
                                      bool changePublicTypesToInternalTypes,
                                      string baseNamespace,
                                      bool includeJetBrainsAnnotations, 
                                      bool removeContractAnnotations, 
                                      bool includeJetBrainsAnnotationsUsing, 
                                      bool includeVersionComment, 
                                      bool removeOverloadsWithExceptionFactory,
                                      bool includeCodeAnalysisNullableAttributes)
        {
            SourceFolder = sourceFolder.MustNotBeNullOrWhiteSpace(nameof(sourceFolder));
            TargetFile = targetFile.MustNotBeNullOrWhiteSpace(nameof(targetFile));
            ChangePublicTypesToInternalTypes = changePublicTypesToInternalTypes;
            BaseNamespace = baseNamespace.MustNotBeNullOrWhiteSpace(nameof(baseNamespace));
            IncludeJetBrainsAnnotations = includeJetBrainsAnnotations;
            RemoveContractAnnotations = removeContractAnnotations;
            IncludeJetBrainsAnnotationsUsing = includeJetBrainsAnnotationsUsing;
            IncludeVersionComment = includeVersionComment;
            RemoveOverloadsWithExceptionFactory = removeOverloadsWithExceptionFactory;
            IncludeCodeAnalysisNullableAttributes = includeCodeAnalysisNullableAttributes;
        }

        public string SourceFolder { get; }

        public string TargetFile { get; }

        public bool ChangePublicTypesToInternalTypes { get; }

        public string BaseNamespace { get; }

        public bool IncludeJetBrainsAnnotations { get; }

        public bool RemoveContractAnnotations { get; }

        public bool IncludeJetBrainsAnnotationsUsing { get; }

        public bool IncludeVersionComment { get; }

        public bool RemoveOverloadsWithExceptionFactory { get; }

        public bool IncludeCodeAnalysisNullableAttributes { get; }

        public sealed class Builder
        {
            public Builder()
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

            public string SourceFolder { get; set; }

            public string TargetFile { get; set; }

            public bool ChangePublicTypesToInternalTypes { get; set; } = true;

            public string BaseNamespace { get; set; } = "Light.GuardClauses";

            public bool RemoveContractAnnotations { get; set; } = false;

            public bool IncludeJetBrainsAnnotations { get; set; } = true;

            public bool IncludeJetBrainsAnnotationsUsing { get; set; } = true;

            public bool IncludeVersionComment { get; set; } = true;

            public bool RemoveOverloadsWithExceptionFactory { get; set; } = false;

            public bool IncludeCodeAnalysisNullableAttributes { get; set; } = true;

            public SourceFileMergeOptions Build() =>
                new SourceFileMergeOptions(
                    SourceFolder,
                    TargetFile,
                    ChangePublicTypesToInternalTypes,
                    BaseNamespace,
                    IncludeJetBrainsAnnotations,
                    RemoveContractAnnotations,
                    IncludeJetBrainsAnnotationsUsing,
                    IncludeVersionComment,
                    RemoveOverloadsWithExceptionFactory,
                    IncludeCodeAnalysisNullableAttributes);
        }
    }
}