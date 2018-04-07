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
    public sealed class FixMessageXmlComment : CodeFixProvider
    {
        public override ImmutableArray<string> FixableDiagnosticIds { get; } =
            ImmutableArray.Create(Descriptors.MessageComment.Id);

        public override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            var syntaxRoot = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
            var diagnostic = context.Diagnostics[0];
            var xmlElementSyntax = (XmlElementSyntax) syntaxRoot.FindNode(diagnostic.Location.SourceSpan, true);
            var documentationSyntax = (DocumentationCommentTriviaSyntax) xmlElementSyntax.Parent;

            context.RegisterCodeFix(CodeAction.Create(diagnostic.Descriptor.Title.ToString(),
                                                      cancellationToken => SetDefaultXmlCommentForMessage(context.Document,
                                                                                                          syntaxRoot,
                                                                                                          xmlElementSyntax,
                                                                                                          documentationSyntax)),
                                    diagnostic);
        }

        private static Task<Document> SetDefaultXmlCommentForMessage(Document document,
                                                                     SyntaxNode syntaxRoot,
                                                                     XmlElementSyntax xmlElementSyntax,
                                                                     DocumentationCommentTriviaSyntax documentationSyntax)
        {
            XmlElementSyntax newCommentSyntax;

            if (xmlElementSyntax.Content.Count < 3)
            {
                var firstExceptionCref = documentationSyntax.GetFirstXmlExceptionCref();

                if (firstExceptionCref == null)
                    newCommentSyntax = xmlElementSyntax.WithContent(new SyntaxList<XmlNodeSyntax>(SyntaxFactory.XmlText(MessageConstants.FullDefaultComment)));
                else
                {
                    var seeElement = SyntaxFactory.XmlSeeElement(firstExceptionCref.Cref);
                    seeElement = seeElement.WithSlashGreaterThanToken(seeElement.SlashGreaterThanToken.WithLeadingTrivia(SyntaxFactory.Whitespace(" ")));
                    newCommentSyntax = xmlElementSyntax.WithContent(new SyntaxList<XmlNodeSyntax>(new XmlNodeSyntax[]
                    {
                        SyntaxFactory.XmlText($"{MessageConstants.CommentStart}the "),
                        seeElement,
                        SyntaxFactory.XmlText(MessageConstants.CommentEnd)
                    }));
                }
            }
            else
            {
                var isFirstContentText = xmlElementSyntax.Content.First() is XmlTextSyntax;
                var isLastContentText = xmlElementSyntax.Content.Last() is XmlTextSyntax;
                var numberOfElements = xmlElementSyntax.Content.Count
                                     + (isFirstContentText ? 0 : 1)
                                     + (isLastContentText ? 0 : 1);
                var commentTextSyntax = new XmlNodeSyntax[numberOfElements];

                var j = isFirstContentText ? 1 : 0;
                for (var i = 1; i < numberOfElements - 1; i++)
                {
                    commentTextSyntax[i] = xmlElementSyntax.Content[j++];
                }

                commentTextSyntax[0] = SyntaxFactory.XmlText($"{MessageConstants.CommentStart}the ");
                commentTextSyntax[numberOfElements - 1] = SyntaxFactory.XmlText(MessageConstants.CommentEnd);
                newCommentSyntax = xmlElementSyntax.WithContent(new SyntaxList<XmlNodeSyntax>(commentTextSyntax));
            }

            return Task.FromResult(document.WithSyntaxRoot(syntaxRoot.ReplaceNode(xmlElementSyntax, newCommentSyntax)));
        }
    }
}