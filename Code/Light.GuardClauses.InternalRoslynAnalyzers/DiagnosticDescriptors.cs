using Microsoft.CodeAnalysis;

namespace Light.GuardClauses.InternalRoslynAnalyzers
{
    public static class DiagnosticDescriptors
    {
        public const string DescriptorIdPrefix = "LightGuardClauses";

        public static readonly DiagnosticDescriptor NonDefaultXmlCommentForParameterName =
            new DiagnosticDescriptor("DefaultCommentForParamaterName".CreateDescriptorId(),
                                     "Default XML comment for parameterName",
                                     "Use the default comment for parameterName: \"The name of the parameter (optional).\"",
                                     "XML Comments",
                                     DiagnosticSeverity.Warning,
                                     true);

        public static readonly DiagnosticDescriptor NonDefaultXmlCommentForMessage =
            new DiagnosticDescriptor("DefaultCommentForMessage".CreateDescriptorId(),
                                     "Default XML comment for message",
                                     "Use the default comment for message: \"The message that will be injected in the resulting exception (optional).\"",
                                     "XML Comments",
                                     DiagnosticSeverity.Warning,
                                     true);

        private static string CreateDescriptorId(this string partialId) => $"{DescriptorIdPrefix}_{partialId}";
    }
}