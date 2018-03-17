namespace Light.GuardClauses.CodeGeneration.LowLevelWriting
{
    public static class AllmanCodeStyle
    {
        public static CodeWriter OpenScopeAndIndent(this CodeWriter codeWriter) =>
            codeWriter.MustNotBeNull(nameof(codeWriter))
                      .WriteLine("{")
                      .IncreaseIndentation();

        public static CodeWriter CloseScope(this CodeWriter codeWriter) =>
            codeWriter.MustNotBeNull(nameof(codeWriter))
                      .DecreaseIndentation()
                      .WriteLine("}");

        public static CodeWriter CloseRemainingScopes(this CodeWriter codeWriter, bool startOnPreviousIndentationLevel = true)
        {
            if (startOnPreviousIndentationLevel)
                codeWriter.DecreaseIndentation();

            while (true)
            {
                codeWriter.WriteLine("}");
                if (codeWriter.IndentationLevel == 0)
                    break;
                codeWriter.DecreaseIndentation();
            }
            return codeWriter;
        }
    }
}