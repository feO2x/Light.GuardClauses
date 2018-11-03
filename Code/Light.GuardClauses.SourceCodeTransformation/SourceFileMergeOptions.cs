using System.Collections.Generic;

namespace Light.GuardClauses.SourceCodeTransformation
{
    public sealed class SourceFileMergeOptions
    {
        public SourceFileMergeOptions(string sourceFolder,
                                      string targetFile,
                                      bool changePublicToInternal,
                                      string baseNamespace,
                                      bool includeJetBrainsAnnotations, 
                                      bool removePreprocessorDirectives,
                                      IReadOnlyList<string> definedPreprocessorSymbols)
        {
            SourceFolder = sourceFolder.MustNotBeNullOrWhiteSpace(nameof(sourceFolder));
            TargetFile = targetFile.MustNotBeNullOrWhiteSpace(nameof(targetFile));
            ChangePublicToInternal = changePublicToInternal;
            BaseNamespace = baseNamespace.MustNotBeNullOrWhiteSpace(nameof(baseNamespace));
            IncludeJetBrainsAnnotations = includeJetBrainsAnnotations;
            RemovePreprocessorDirectives = removePreprocessorDirectives;
            if (removePreprocessorDirectives)
                DefinedPreprocessorSymbols = definedPreprocessorSymbols.MustNotBeNull(nameof(definedPreprocessorSymbols));
        }

        public string SourceFolder { get; }

        public string TargetFile { get; }

        public bool ChangePublicToInternal { get; }

        public string BaseNamespace { get; }

        public bool IncludeJetBrainsAnnotations { get; }

        public bool RemovePreprocessorDirectives { get; }

        public IReadOnlyList<string> DefinedPreprocessorSymbols { get; }
    }
}