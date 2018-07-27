using Microsoft.CodeAnalysis;

namespace Light.GuardClauses.InternalRoslynAnalyzers
{
    public static class Descriptors
    {
        public const string DescriptorIdPrefix = "LightGuardClauses";

        public const string XmlCommentsCategory = "XML Comments";

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
                                     $"Use the default comment for message: \"{MessageConstants.DefaultComment}\"",
                                     XmlCommentsCategory,
                                     DiagnosticSeverity.Warning,
                                     true);

        private static string CreateDescriptorId(this string partialId) => $"{DescriptorIdPrefix}_{partialId}";
    }
}