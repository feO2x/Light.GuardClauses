using Microsoft.CodeAnalysis;

namespace Light.GuardClauses.InternalRoslynAnalyzers
{
    public static class Descriptors
    {
        public const string DescriptorIdPrefix = "LightGuardClauses";

        public const string XmlCommentsCategory = "XML Comments";

        public const string PublicApiCategory = "Public API";

        public static readonly DiagnosticDescriptor ParameterNameComment =
            new DiagnosticDescriptor("ParameterNameComment".CreateDescriptorId(),
                                     "Use default XML comment for parameterName",
                                     $"Use the default comment for parameterName: \"{ParameterNameConstants.DefaultComment}\"",
                                     XmlCommentsCategory,
                                     DiagnosticSeverity.Warning,
                                     true);

        public static readonly DiagnosticDescriptor MessageComment =
            new DiagnosticDescriptor("MessageComment".CreateDescriptorId(),
                                     "Use default XML comment for message",
                                     $"Use the default comment for message: \"{MessageConstants.FullDefaultComment}\"",
                                     XmlCommentsCategory,
                                     DiagnosticSeverity.Warning,
                                     true);

        public static readonly DiagnosticDescriptor CustomExceptionOverload =
            new DiagnosticDescriptor("SimpleCustomExceptionOverload".CreateDescriptorId(),
                                     "Create an overload that allows customizing the exception.",
                                     "Create an overload that allows customizing the exception.",
                                     PublicApiCategory,
                                     DiagnosticSeverity.Warning,
                                     true);

        private static string CreateDescriptorId(this string partialId) => $"{DescriptorIdPrefix}_{partialId}";
    }
}