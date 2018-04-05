using System;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Light.GuardClauses.InternalRoslynAnalyzers
{
    public static class Extensions
    {
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
                if (currentParameter.Type.SpecialType != SpecialType.System_String)
                    continue;

                if (currentParameter.Name.Equals(Ids.ParameterName))
                    wasParameterNameFound = true;
                else if (currentParameter.Name.Equals(Ids.Message))
                    wasMessageFound = true;
            }

            return wasParameterNameFound && wasMessageFound;
        }

        public static void ReportNonDefaultXmlParamComment(this SymbolAnalysisContext context, 
                                                           DocumentationCommentTriviaSyntax documentationSyntax, 
                                                           string xmlParamName, 
                                                           string expectedComment,
                                                           Func<XmlElementSyntax, Diagnostic> createDiagnostic)
        {
            var xmlCommentElement = documentationSyntax.GetXmlCommentParam(xmlParamName);
            if (xmlCommentElement == null)
                return;

            var commentTextSyntax = xmlCommentElement.GetCommentTextSyntax();

            if (commentTextSyntax.EqualsSimpleComment(expectedComment))
                return;

            context.ReportDiagnostic(createDiagnostic(xmlCommentElement));
        }

        public static XmlElementSyntax GetXmlCommentParam(this DocumentationCommentTriviaSyntax documentationSyntax, string xmlParamName) =>
            documentationSyntax.DescendantNodes()
                               .OfType<XmlElementSyntax>()
                               .FirstOrDefault(xmlElementSyntax => xmlElementSyntax.DescendantNodes()
                                                                                    .OfType<XmlNameAttributeSyntax>()
                                                                                    .FirstOrDefault()
                                                                                   ?.Identifier
                                                                                   ?.Identifier
                                                                                    .Text
                                                                                    .Equals(xmlParamName) == true);

        public static XmlTextSyntax GetCommentTextSyntax(this XmlElementSyntax xmlCommentParam) =>
            xmlCommentParam.DescendantNodes()
                           .OfType<XmlTextSyntax>()
                           .FirstOrDefault();

        public static bool EqualsSimpleComment(this XmlTextSyntax commentTextSyntax, string expectedComment) => 
            commentTextSyntax?.TextTokens.Count == 1 && commentTextSyntax.TextTokens[0].Text.Equals(expectedComment);
    }
}