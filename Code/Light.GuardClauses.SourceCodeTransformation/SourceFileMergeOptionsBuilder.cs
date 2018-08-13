using System.IO;

namespace Light.GuardClauses.SourceCodeTransformation
{
    public sealed class SourceFileMergeOptionsBuilder
    {
        public SourceFileMergeOptionsBuilder()
        {
            try
            {
                var currentDirectory = new DirectoryInfo(@".\");
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

        public SourceFileMergeOptions Build() =>
            new SourceFileMergeOptions(SourceFolder, TargetFile, ChangePublicToInternal);
    }
}