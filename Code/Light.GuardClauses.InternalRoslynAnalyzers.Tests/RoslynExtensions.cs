using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
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
            var projectId = ProjectId.CreateNewId(projectName);
            var workspace = new AdhocWorkspace();
            var fileName = $"{projectName}Source.cs";
            var compilation = await workspace.CurrentSolution
                                             .AddProject(projectId, projectName, projectName, LanguageNames.CSharp)
                                             .AddMetadataReferences(projectId, AllReferences)
                                             .AddDocument(DocumentId.CreateNewId(projectId, fileName), fileName, SourceText.From(code))
                                             .GetProject(projectId)
                                             .WithCompilationOptions(new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary))
                                             .GetCompilationAsync();

            return await compilation.WithAnalyzers(ImmutableArray.Create(analyzer))
                                    .GetAllDiagnosticsAsync();
        }
    }
}