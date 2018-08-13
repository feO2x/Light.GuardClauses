namespace Light.GuardClauses.SourceCodeTransformation
{
    public sealed class SourceFileMergeOptions
    {
        public SourceFileMergeOptions(string sourceFolder, string targetFile, bool changePublicToInternal)
        {
            SourceFolder = sourceFolder.MustNotBeNullOrWhiteSpace(nameof(sourceFolder));
            TargetFile = targetFile.MustNotBeNullOrWhiteSpace(nameof(targetFile));
            ChangePublicToInternal = changePublicToInternal;
        }

        public string SourceFolder { get; }

        public string TargetFile { get; }

        public bool ChangePublicToInternal { get; }
    }
}