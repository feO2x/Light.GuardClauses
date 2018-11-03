using System.Collections.Generic;
using System.IO;

namespace Light.GuardClauses.SourceCodeTransformation
{
    public sealed class SourceFileMergeOptionsBuilder
    {
        public SourceFileMergeOptionsBuilder()
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

        public bool ChangePublicToInternal { get; set; } = true;

        public string BaseNamespace { get; set; } = "Light.GuardClauses";

        public bool IncludeJetBrainsAnnotations { get; set; } = true;

        public bool UndefinePreprocessorDirectives { get; set; } = true;

        public List<string> DefinedPreprocessorSymbolsForUndefine { get; set; } = new List<string>{ "NETSTANDARD2_0" };

        public SourceFileMergeOptions Build() =>
            new SourceFileMergeOptions(
                SourceFolder, 
                TargetFile, 
                ChangePublicToInternal, 
                BaseNamespace,
                IncludeJetBrainsAnnotations, 
                UndefinePreprocessorDirectives,
                DefinedPreprocessorSymbolsForUndefine);
    }
}