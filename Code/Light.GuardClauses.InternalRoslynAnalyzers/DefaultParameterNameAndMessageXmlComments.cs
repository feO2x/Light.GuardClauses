using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Light.GuardClauses.InternalRoslynAnalyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public sealed class DefaultParameterNameAndMessageXmlComments : DiagnosticAnalyzer
    {
        private static readonly ImmutableArray<DiagnosticDescriptor> InternalSupportedDiagnostics =
            ImmutableArray.CreateRange(new[]
            {
                DiagnosticDescriptors.NonDefaultXmlCommentForParameterName,
                DiagnosticDescriptors.NonDefaultXmlCommentForMessage
            });


        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => InternalSupportedDiagnostics;

        public override void Initialize(AnalysisContext context)
        {
            context.RegisterSymbolAction(AnalyzeXmlCommentsForParameterNameAndMessage, SymbolKind.Method);
        }

        private static void AnalyzeXmlCommentsForParameterNameAndMessage(SymbolAnalysisContext symbolAnalysisContext)
        {
            var methodSymbol = (IMethodSymbol) symbolAnalysisContext.Symbol;

            // Check if the method is a static assertion method that throws an exception (these start with "Must" by convention) 
            // and if it contains the parameters paramterName and message.
            if (methodSymbol.MethodKind != MethodKind.Ordinary ||
                methodSymbol.IsStatic == false ||
                methodSymbol.Name.StartsWith("Must") == false ||
                methodSymbol.Parameters.ContainsParameterNameAndMessage() == false)
                return;

            // Get the syntax tree of the method
            if (methodSymbol.DeclaringSyntaxReferences.Length != 1 ||
                !(methodSymbol.DeclaringSyntaxReferences[0].GetSyntax() is MethodDeclarationSyntax methodDeclarationSyntax))
                return;

            // Get XML comment trivia
            if (!(methodDeclarationSyntax.DescendantTrivia()
                                         .SingleOrDefault(trivia => trivia.Kind() == SyntaxKind.SingleLineDocumentationCommentTrivia)
                                         .GetStructure() is DocumentationCommentTriviaSyntax documentationSyntax))
                return;

            // Check parameterName
            symbolAnalysisContext.ReportNonDefaultXmlParamComment(documentationSyntax,
                                                                  "parameterName",
                                                                  "The name of the parameter (optional).",
                                                                  xmlComment => Diagnostic.Create(DiagnosticDescriptors.NonDefaultXmlCommentForParameterName, xmlComment.GetLocation()));
            // Check message
            symbolAnalysisContext.ReportNonDefaultXmlParamComment(documentationSyntax,
                                                                  "message",
                                                                  "The message that will be injected in the resulting exception (optional).",
                                                                  xmlComment => Diagnostic.Create(DiagnosticDescriptors.NonDefaultXmlCommentForMessage, xmlComment.GetLocation()));
        }
    }
}