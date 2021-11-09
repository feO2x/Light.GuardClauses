using System;
using System.IO;
using FluentAssertions;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Xunit;

namespace Light.GuardClauses.SourceCodeTransformation.Tests
{
    public static class ParseCSharpWithRoslynTests
    {
        private static readonly DirectoryInfo CodeDirectory;

        static ParseCSharpWithRoslynTests()
        {
            CodeDirectory = FindCodeDirectory();
        }

        [Fact]
        public static void ParseExpressionExtensionsFile()
        {
            var fileInfo = GetLightGuardClausesFile(@"FrameworkExtensions\ExpressionExtensions.cs");
            var syntaxTree = CSharpSyntaxTree.ParseText(File.ReadAllText(fileInfo.FullName));
            var root = (CompilationUnitSyntax) syntaxTree.GetRoot();

            root.Members.Should().NotBeEmpty();
        }

        [Fact]
        public static void ParseSpanDelegatesFile()
        {
            var fileInfo = GetLightGuardClausesFile("SpanDelegates.cs");
            var syntaxTree = CSharpSyntaxTree.ParseText(File.ReadAllText(fileInfo.FullName), new CSharpParseOptions(LanguageVersion.CSharp7_3, preprocessorSymbols: new[] { "NETSTANDARD2_0" }));
            var root = (CompilationUnitSyntax) syntaxTree.GetRoot();

            root.Members.Should().NotBeEmpty();
        }

        private static DirectoryInfo FindCodeDirectory()
        {
            var currentDirectory = new DirectoryInfo(".");
            do
            {
                if (currentDirectory.Name == "Code")
                    return currentDirectory;

                currentDirectory = currentDirectory.Parent;
            } while (currentDirectory != null);

            throw new InvalidOperationException("This test project does not reside in a folder called \"Code\" (directly or indirectly).");
        }

        private static FileInfo GetLightGuardClausesFile(string relativeFilePath)
        {
            return new FileInfo(Path.Combine(CodeDirectory.FullName, "Light.GuardClauses", relativeFilePath));
        }
    }
}