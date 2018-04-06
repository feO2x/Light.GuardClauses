namespace Light.GuardClauses.InternalRoslynAnalyzers
{
    public static class ParameterNameDefaults
    {
        public const string ParameterName = "parameterName";
        public const string DefaultComment = "The name of the parameter (optional).";
    }

    public static class MessageDefaults
    {
        public const string ParameterName = "message";
        public const string FullDefaultComment = CommentStart + "the resulting exception" + CommentEnd;
        public const string CommentStart = "The message that will be passed to ";
        public const string CommentEnd = " (optional).";
    }
}