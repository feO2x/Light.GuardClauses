using System.Collections.Immutable;
using System.Composition;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Light.GuardClauses.InternalRoslynAnalyzers
{
    [ExportCodeFixProvider(LanguageNames.CSharp), Shared]
    public sealed class MessageXmlCommentFix : CodeFixProvider
    {
        public override ImmutableArray<string> FixableDiagnosticIds { get; } =
            ImmutableArray.Create(Descriptors.MessageComment.Id);

        public override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            var syntaxRoot = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
            var diagnostic = context.Diagnostics[0];
            var xmlElementSyntax = (XmlElementSyntax) syntaxRoot.FindNode(diagnostic.Location.SourceSpan, true);

            var title = diagnostic.Descriptor.Title.ToString();
            context.RegisterCodeFix(CodeAction.Create(title,
                                                      cancellationToken => SetDefaultXmlCommentForMessage(context.Document,
                                                                                                          syntaxRoot,
                                                                                                          xmlElementSyntax),
                                                      title),
                                    diagnostic);
        }

        public override FixAllProvider GetFixAllProvider() => null;

        private static Task<Document> SetDefaultXmlCommentForMessage(Document document, SyntaxNode syntaxRoot, XmlElementSyntax xmlElementSyntax) =>
            Task.FromResult(
                document.WithSyntaxRoot(
                    syntaxRoot.ReplaceNode(
                        xmlElementSyntax,
                        xmlElementSyntax
                           .WithContent(
                                new SyntaxList<XmlNodeSyntax>(
                                    XmlText(MessageConstants.DefaultComment))))));
    }
}