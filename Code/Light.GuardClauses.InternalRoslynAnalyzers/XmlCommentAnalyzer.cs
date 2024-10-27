using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Light.GuardClauses.InternalRoslynAnalyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public sealed class XmlCommentAnalyzer : DiagnosticAnalyzer
    {
        private static readonly ImmutableArray<DiagnosticDescriptor> InternalSupportedDiagnostics =
            ImmutableArray.CreateRange(new[]
            {
                Descriptors.ParameterNameComment,
                Descriptors.MessageComment
            });


        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => InternalSupportedDiagnostics;

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.Analyze | GeneratedCodeAnalysisFlags.ReportDiagnostics);
            context.EnableConcurrentExecution();
            context.RegisterSymbolAction(AnalyzeXmlCommentsForParameterNameAndMessage, SymbolKind.Method);
        }

        private static void AnalyzeXmlCommentsForParameterNameAndMessage(SymbolAnalysisContext symbolAnalysisContext)
        {
            var methodSymbol = (IMethodSymbol) symbolAnalysisContext.Symbol;

            if (!methodSymbol.IsStandardExceptionAssertion())
                return;

            if (methodSymbol.DeclaringSyntaxReferences.Length != 1 ||
                !(methodSymbol.DeclaringSyntaxReferences[0].GetSyntax() is MethodDeclarationSyntax methodDeclarationSyntax) ||
                !(methodDeclarationSyntax.DescendantTrivia()
                                         .SingleOrDefault(trivia => trivia.IsKind(SyntaxKind.SingleLineDocumentationCommentTrivia))
                                         .GetStructure() is DocumentationCommentTriviaSyntax documentationSyntax))
                return;

            symbolAnalysisContext.ReportNonDefaultParameterNameComment(documentationSyntax);
            symbolAnalysisContext.ReportMessageComment(documentationSyntax);
        }
    }
}