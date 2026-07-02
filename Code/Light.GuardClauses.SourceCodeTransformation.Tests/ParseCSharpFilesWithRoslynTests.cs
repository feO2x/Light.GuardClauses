using System.IO;
using FluentAssertions;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Xunit;

namespace Light.GuardClauses.SourceCodeTransformation.Tests;

public static class ParseCSharpWithRoslynTests
{
    [Fact]
    public static void ParseExpressionExtensionsFile()
    {
        var fileInfo = GetLightGuardClausesFile("FrameworkExtensions/ExpressionExtensions.cs");
        var syntaxTree = CSharpSyntaxTree.ParseText(File.ReadAllText(fileInfo.FullName));
        var root = (CompilationUnitSyntax) syntaxTree.GetRoot();

        root.Members.Should().NotBeEmpty();
    }

    [Fact]
    public static void ParseSpanDelegatesFile()
    {
        var fileInfo = GetLightGuardClausesFile("ExceptionFactory/SpanDelegates.cs");
        var syntaxTree = CSharpSyntaxTree.ParseText(
            File.ReadAllText(fileInfo.FullName),
            new (LanguageVersion.CSharp7_3, preprocessorSymbols: ["NETSTANDARD2_0"])
        );
        var root = (CompilationUnitSyntax) syntaxTree.GetRoot();

        root.Members.Should().NotBeEmpty();
    }

    private static FileInfo GetLightGuardClausesFile(string relativeFilePath) => new (
        Path.Combine(TestEnvironment.SourceDirectory.FullName, relativeFilePath)
    );
}
