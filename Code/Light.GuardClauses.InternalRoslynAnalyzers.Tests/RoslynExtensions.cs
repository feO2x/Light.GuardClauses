using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Text;

namespace Light.GuardClauses.InternalRoslynAnalyzers.Tests
{
    public static class RoslynExtensions
    {
        public static readonly MetadataReference MsCoreLibReference = MetadataReference.CreateFromFile(typeof(object).Assembly.Location);
        public static readonly MetadataReference SystemCoreReference = MetadataReference.CreateFromFile(typeof(Enumerable).Assembly.Location);
        public static readonly MetadataReference CompilationReference = MetadataReference.CreateFromFile(typeof(Compilation).Assembly.Location);
        public static readonly MetadataReference CSharpCompilationReference = MetadataReference.CreateFromFile(typeof(CSharpCompilation).Assembly.Location);

        public static readonly IReadOnlyList<MetadataReference> AllReferences =
            new[]
            {
                MsCoreLibReference,
                SystemCoreReference,
                CompilationReference,
                CSharpCompilationReference
            };

        public static async Task<ImmutableArray<Diagnostic>> AnalyzeAsync(this DiagnosticAnalyzer analyzer, string code, [CallerMemberName] string projectName = null)
        {
            var (_, compilation) = await CreateLightGuardClausesDllInMemory(projectName, code);

            return await compilation.WithAnalyzers(ImmutableArray.Create(analyzer))
                                    .GetAllDiagnosticsAsync();
        }

        public static async Task<string> ApplyFixAsync(this CodeFixProvider codeFixProvider,
                                                       string code,
                                                       DiagnosticAnalyzer analyzer,
                                                       [CallerMemberName] string projectName = null)
        {
            var (document, compilation) = await CreateLightGuardClausesDllInMemory(projectName, code);

            var diagnostics = await compilation.WithAnalyzers(ImmutableArray.Create(analyzer))
                                               .GetAllDiagnosticsAsync();
            var codeActions = new List<CodeAction>();
            var context = new CodeFixContext(document,
                                             diagnostics[0],
                                             (action, diagnostic) => codeActions.Add(action),
                                             CancellationToken.None);
            await codeFixProvider.RegisterCodeFixesAsync(context);
            document = (await codeActions[0].GetOperationsAsync(CancellationToken.None)).OfType<ApplyChangesOperation>()
                                                                                        .Single()
                                                                                        .ChangedSolution
                                                                                        .GetDocument(document.Id);

            return (await document.GetSyntaxRootAsync()).GetText().ToString();
        }

        public static async Task<(Document, Compilation)> CreateLightGuardClausesDllInMemory(string projectName, string code)
        {
            var projectId = ProjectId.CreateNewId(projectName);
            var codeFileName = $"{projectName}Source.cs";
            var codeFileId = DocumentId.CreateNewId(projectId, codeFileName);
            var project = new AdhocWorkspace().CurrentSolution
                                              .AddProject(projectId, projectName, projectName, LanguageNames.CSharp)
                                              .AddMetadataReferences(projectId, AllReferences)
                                              .AddDocument(codeFileId, codeFileName, SourceText.From(code))
                                              .AddThrowClass(projectId)
                                              .GetProject(projectId)
                                              .WithCompilationOptions(new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
            var document = project.GetDocument(codeFileId);
            var compilation = await project.GetCompilationAsync();

            return (document, compilation);
        }

        private static Solution AddThrowClass(this Solution solution, ProjectId projectId)
        {
            const string fileName = "Throw.cs";
            return solution.AddDocument(DocumentId.CreateNewId(projectId, fileName), fileName, SourceText.From(File.ReadAllText(fileName)));
        }
    }
}