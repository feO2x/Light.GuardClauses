using System.Collections.Immutable;
using System.Composition;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Light.GuardClauses.InternalRoslynAnalyzers
{
    [ExportCodeFixProvider(LanguageNames.CSharp), Shared]
    public sealed class FixParameterNameXmlComment : CodeFixProvider
    {
        public override ImmutableArray<string> FixableDiagnosticIds { get; } =
            ImmutableArray.Create(Descriptors.ParameterNameComment.Id);

        public override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            var syntaxRoot = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
            var diagnostic = context.Diagnostics[0];
            var xmlElementSyntax = (XmlElementSyntax) syntaxRoot.FindNode(diagnostic.Location.SourceSpan, true);

            context.RegisterCodeFix(CodeAction.Create(diagnostic.Descriptor.Title.ToString(),
                                                      cancellationToken => SetDefaultXmlCommentForParameterName(context.Document,
                                                                                                                syntaxRoot,
                                                                                                                xmlElementSyntax)),
                                    diagnostic);
        }

        private static Task<Document> SetDefaultXmlCommentForParameterName(Document document, SyntaxNode syntaxRoot, XmlElementSyntax xmlElementSyntax) =>
            Task.FromResult(document.WithSyntaxRoot(syntaxRoot.ReplaceNode(xmlElementSyntax,
                                                                           xmlElementSyntax.WithContent(new SyntaxList<XmlNodeSyntax>(SyntaxFactory.XmlText(ParameterNameDefaults.DefaultComment))))));
    }
}