using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Light.GuardClauses.InternalRoslynAnalyzers
{
    internal static class Extensions
    {
        public static bool IsStandardExceptionAssertion(this IMethodSymbol methodSymbol) =>
            methodSymbol.MethodKind == MethodKind.Ordinary &&
            methodSymbol.IsStatic &&
            methodSymbol.Name.StartsWith("Must") &&
            methodSymbol.Parameters.ContainsParameterNameAndMessage();

        public static bool ContainsParameterNameAndMessage(this ImmutableArray<IParameterSymbol> parameters)
        {
            // The method must have at least 3 parameters (the assertion target, parameterName, and message)
            if (parameters.Length < 3)
                return false;

            var wasParameterNameFound = false;
            var wasMessageFound = false;

            for (var i = 0; i < parameters.Length; ++i)
            {
                var currentParameter = parameters[i];
                if (currentParameter.Type.SpecialType != SpecialType.System_String || !currentParameter.IsOptional)
                    continue;

                if (currentParameter.Name.Equals(ParameterNameConstants.ParameterName))
                    wasParameterNameFound = true;
                else if (currentParameter.Name.Equals(MessageConstants.ParameterName))
                    wasMessageFound = true;
            }

            return wasParameterNameFound && wasMessageFound;
        }

        public static void ReportNonDefaultParameterNameComment(this SymbolAnalysisContext context,
                                                                DocumentationCommentTriviaSyntax documentationSyntax)
        {
            var xmlCommentElement = documentationSyntax.GetXmlCommentParam(ParameterNameConstants.ParameterName);
            if (xmlCommentElement == null)
                return;

            if (xmlCommentElement.GetSimpleCommentText().EqualsString(ParameterNameConstants.DefaultComment))
                return;

            context.ReportDiagnostic(Diagnostic.Create(Descriptors.ParameterNameComment, xmlCommentElement.GetLocation()));
        }

        public static void ReportMessageComment(this SymbolAnalysisContext context,
                                                DocumentationCommentTriviaSyntax documentationSyntax)
        {
            var xmlCommentElement = documentationSyntax.GetXmlCommentParam(MessageConstants.ParameterName);
            if (xmlCommentElement == null)
                return;

            if (xmlCommentElement.GetSimpleCommentText().EqualsString(MessageConstants.DefaultComment))
                return;

            context.ReportDiagnostic(Diagnostic.Create(Descriptors.MessageComment, xmlCommentElement.GetLocation()));
        }

        public static XmlElementSyntax GetXmlCommentParam(this DocumentationCommentTriviaSyntax documentationSyntax, string xmlParamName) =>
            documentationSyntax?.ChildNodes()
                                .OfType<XmlElementSyntax>()
                                .FirstOrDefault(xmlElementSyntax => xmlElementSyntax.IsXmlCommentParam(xmlParamName));

        public static bool IsXmlCommentParam(this XmlElementSyntax xmlElement, string xmlParamName) =>
            xmlElement.StartTag.Name.LocalName.Text == "param" &&
            xmlElement.StartTag.Attributes.Any(attribute => attribute is XmlNameAttributeSyntax nameAttribute &&
                                                            nameAttribute.Identifier.Identifier.Text == xmlParamName);

        private static XmlTextSyntax GetSimpleCommentText(this XmlElementSyntax xmlCommentParam) =>
            xmlCommentParam.Content.Count != 1 ? null : xmlCommentParam.Content[0] as XmlTextSyntax;


        public static bool EqualsString(this XmlTextSyntax commentTextSyntax, string value) =>
            commentTextSyntax?.TextTokens.Count == 1 && commentTextSyntax.TextTokens[0].Text.Equals(value);
    }
}