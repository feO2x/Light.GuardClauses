using System.Collections.Generic;

namespace Light.GuardClauses.SourceCodeTransformation
{
    public sealed class SourceFileMergeOptions
    {
        public SourceFileMergeOptions(string sourceFolder,
                                      string targetFile,
                                      bool changePublicTypesToInternalTypes,
                                      string baseNamespace,
                                      bool includeJetBrainsAnnotations, 
                                      bool removePreprocessorDirectives,
                                      IReadOnlyList<string> definedPreprocessorSymbols,
                                      bool removeContractAnnotations)
        {
            SourceFolder = sourceFolder.MustNotBeNullOrWhiteSpace(nameof(sourceFolder));
            TargetFile = targetFile.MustNotBeNullOrWhiteSpace(nameof(targetFile));
            ChangePublicTypesToInternalTypes = changePublicTypesToInternalTypes;
            BaseNamespace = baseNamespace.MustNotBeNullOrWhiteSpace(nameof(baseNamespace));
            IncludeJetBrainsAnnotations = includeJetBrainsAnnotations;
            RemovePreprocessorDirectives = removePreprocessorDirectives;
            if (removePreprocessorDirectives)
                DefinedPreprocessorSymbols = definedPreprocessorSymbols.MustNotBeNull(nameof(definedPreprocessorSymbols));
            RemoveContractAnnotations = removeContractAnnotations;
        }

        public string SourceFolder { get; }

        public string TargetFile { get; }

        public bool ChangePublicTypesToInternalTypes { get; }

        public string BaseNamespace { get; }

        public bool IncludeJetBrainsAnnotations { get; }

        public bool RemovePreprocessorDirectives { get; }

        public IReadOnlyList<string> DefinedPreprocessorSymbols { get; }

        public bool RemoveContractAnnotations { get; }
    }
}