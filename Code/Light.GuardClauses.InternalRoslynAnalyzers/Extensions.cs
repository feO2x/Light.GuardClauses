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

        public static void ReportNonDefaultParameterNameComment(this SymbolAnalysisContext context,
                                                                DocumentationCommentTriviaSyntax documentationSyntax)
        {
            var xmlCommentElement = documentationSyntax.GetXmlCommentParam(ParameterNameDefaults.ParameterName);
            if (xmlCommentElement == null)
                return;

            if (xmlCommentElement.GetSimpleCommentText().EqualsString(ParameterNameDefaults.DefaultComment))
                return;

            context.ReportDiagnostic(Diagnostic.Create(Descriptors.ParameterNameComment, xmlCommentElement.GetLocation()));
        }

        public static void ReportMessageComment(this SymbolAnalysisContext context,
                                                DocumentationCommentTriviaSyntax documentationSyntax)
        {
            var xmlCommentElement = documentationSyntax.GetXmlCommentParam(MessageDefaults.ParameterName);
            if (xmlCommentElement == null)
                return;

            // The following cases report a non-default message diagnostic:
            if ( // There is no comment text at all
                xmlCommentElement.Content.Count == 0 ||

                // The first content element is no XML text
                !(xmlCommentElement.Content[0] is XmlTextSyntax firstTextSyntax) ||

                // There is only XML text but it does not equal the full default comment
                xmlCommentElement.Content.Count == 1 &&
                !firstTextSyntax.StartAndEndsWith(MessageDefaults.CommentStart, MessageDefaults.CommentEnd) ||

                // There are two content nodes (most probably the "(optional)." is missing)
                xmlCommentElement.Content.Count == 2 ||

                // The first element is not the start of the default message comment "The message that is passed to"
                !firstTextSyntax.StartsWith(MessageDefaults.CommentStart) ||

                // The last element is not the end of the default message comment " (optional)."
                !(xmlCommentElement.Content.Last() is XmlTextSyntax endTextSyntax) ||
                !endTextSyntax.EndsWith(MessageDefaults.CommentEnd))
            {
                context.ReportDiagnostic(Diagnostic.Create(Descriptors.MessageComment, xmlCommentElement.GetLocation()));
            }
        }

        public static XmlElementSyntax GetXmlCommentParam(this DocumentationCommentTriviaSyntax documentationSyntax, string xmlParamName) =>
            documentationSyntax?.ChildNodes()
                                .OfType<XmlElementSyntax>()
                                .FirstOrDefault(xmlElementSyntax => xmlElementSyntax.StartTag.Name.LocalName.Text == "param" &&
                                                                    xmlElementSyntax.StartTag.Attributes.Any(attribute => attribute is XmlNameAttributeSyntax nameAttribute &&
                                                                                                                          nameAttribute.Identifier.Identifier.Text == xmlParamName));

        private static XmlTextSyntax GetSimpleCommentText(this XmlElementSyntax xmlCommentParam) =>
            xmlCommentParam.Content.Count != 1 ? null : xmlCommentParam.Content[0] as XmlTextSyntax;


        public static bool EqualsString(this XmlTextSyntax commentTextSyntax, string value) =>
            commentTextSyntax?.TextTokens.Count == 1 && commentTextSyntax.TextTokens[0].Text.Equals(value);

        public static bool StartsWith(this XmlTextSyntax commentTextSyntax, string value) =>
            commentTextSyntax?.TextTokens.Count == 1 && commentTextSyntax.TextTokens[0].Text.StartsWith(value);

        public static bool EndsWith(this XmlTextSyntax commentTextSyntax, string value) =>
            commentTextSyntax?.TextTokens.Count == 1 && commentTextSyntax.TextTokens[0].Text.EndsWith(value);

        public static bool StartAndEndsWith(this XmlTextSyntax commentTextSyntax, string start, string end)
        {
            if (commentTextSyntax == null ||
                commentTextSyntax.TextTokens.Count != 0)
                return false;

            var text = commentTextSyntax.TextTokens[0].Text;
            return text.StartsWith(start) && text.EndsWith(end);
        }
            

        public static XmlCrefAttributeSyntax GetFirstXmlExceptionCref(this DocumentationCommentTriviaSyntax documentationSyntax) =>
            documentationSyntax.ChildNodes()
                               .OfType<XmlElementSyntax>()
                               .FirstOrDefault(xmlElementSyntax => xmlElementSyntax.StartTag.Name.LocalName.Text == "exception")
                              ?.StartTag
                               .Attributes
                               .OfType<XmlCrefAttributeSyntax>()
                               .FirstOrDefault();
    }
}