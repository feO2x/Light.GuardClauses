namespace Light.GuardClauses.CodeGeneration.LowLevelWriting
{
    public static class XmlComments
    {
        public static CodeWriter WriteXmlCommentSummary(this CodeWriter codeWriter, string summary) =>
            codeWriter.MustNotBeNull(nameof(codeWriter))
                      .WriteLine("/// <summary>")
                      .WriteLine($"/// {summary}")
                      .WriteLine("/// </summary>");

        public static CodeWriter WriteXmlCommentParam(this CodeWriter codeWriter, string parameterName, string comment) =>
            codeWriter.MustNotBeNull(nameof(codeWriter))
                      .WriteLine($"/// <param name=\"{parameterName}\">{comment}</param>");

        public static CodeWriter WriteXmlCommentException(this CodeWriter codeWriter, string exceptionType, string comment) =>
            codeWriter.MustNotBeNull(nameof(codeWriter))
                      .WriteLine($"/// <exception cref=\"{exceptionType}\">{comment}</exception>");

        public static CodeWriter WriteXmlCommentReturns(this CodeWriter codeWriter, string description) =>
            codeWriter.MustNotBeNull(nameof(codeWriter))
                      .WriteLine($"/// <returns>{description}</returns>");

        public static string ToParamRef(string parameter)
        {
            parameter.MustNotBeNullOrWhiteSpace(nameof(parameter));
            return $"<paramref name=\"{parameter}\" />";
        }

        public static string ToSee(string reference)
        {
            reference.MustNotBeNullOrWhiteSpace(nameof(reference));
            return $"<see cref=\"{reference}\" />";
        }
    }
}