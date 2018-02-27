#! "netcoreapp2.0"
#r "nuget: Light.GuardClauses, 3.5.0"

using Light.GuardClauses;

public sealed class CSharpCodeWriter
{
    private readonly TextWriter _writer;
    private int _indentationLevel;
    private readonly string _indentationCharacters;
    private bool _isCurrentLineEmpty = true;

    public CSharpCodeWriter(TextWriter writer, int initialIndentationLevel = 0, string indentationCharacters = "    ")
    {
        _writer = writer.MustNotBeNull(nameof(writer));
        _indentationLevel = initialIndentationLevel.MustBeGreaterThanOrEqualTo(0, nameof(initialIndentationLevel));
        _indentationCharacters = indentationCharacters.MustNotBeNull(nameof(indentationCharacters));
    }

    public int IndentationLevel => _indentationLevel;

    public string indentationCharacters => _indentationCharacters;

    public CSharpCodeWriter IncreaseIndentation()
    {
        ++_indentationLevel;
        return this;
    }

    public CSharpCodeWriter DecreaseIndentation()
    {
        _indentationLevel.MustBeGreaterThan(0, exception: () => new InvalidOperationException("The indentation level cannot be less than 0."));

        --_indentationLevel;
        return this;
    }

    public CSharpCodeWriter Write(string code)
    {
        WriteIndentationIfNecessary();
        _writer.Write(code);
        return this;
    }

    public CSharpCodeWriter WriteLine(string code)
    {
        Write(code);
        _writer.WriteLine();
        _isCurrentLineEmpty = true;
        return this;
    }

    public CSharpCodeWriter OpenScopeAndIndent() => WriteLine("{").IncreaseIndentation();

    public CSharpCodeWriter CloseScope() => DecreaseIndentation().WriteLine("}");

    public CSharpCodeWriter WriteEmptyLine() => WriteLine(string.Empty);

    public CSharpCodeWriter IncludeNamespace(string @namespace) => WriteLine($"using {@namespace};");

    public CSharpCodeWriter OpenNamespace(string @namespace) => WriteLine($"namespace {@namespace}").OpenScopeAndIndent();

    public CSharpCodeWriter OpenPublicStaticPartialClass(string className) => WriteLine($"public static partial class {className}").OpenScopeAndIndent();

    public CSharpCodeWriter WriteAggressiveInliningAttribute() => WriteLine("[MethodImpl(MethodImplOptions.AggressiveInlining)]");

    public CSharpCodeWriter OpenMember(string memberHeader) => WriteLine(memberHeader).OpenScopeAndIndent();

    public CSharpCodeWriter CloseRemainingScopes(bool startOnPreviousIndentationLevel = true)
    {
        if (startOnPreviousIndentationLevel)
            DecreaseIndentation();

        while(true)
        {
            WriteLine("}");
            if (_indentationLevel == 0)
                break;
            DecreaseIndentation();
        }
        return this;
    }

    public CSharpCodeWriter WriteCodeGenerationNotice(string generatorName) =>
        WriteLine($@"
// PLEASE NOTICE: this code is auto-generated. Any changes you make to this file are going to be overwritten when this file is recreated by the code generator.
// Check ""{generatorName}"" for details.")
       .WriteEmptyLine();

    public CSharpCodeWriter WriteXmlCommentSummary(string summary) => WriteLine("/// <summary>")
                                                                     .WriteLine($"/// {summary}")
                                                                     .WriteLine("/// </summary>");
    public CSharpCodeWriter WriteXmlCommentParam(string parameterName, string comment) => WriteLine($"/// <param name=\"{parameterName}\">{comment}</param>");
    
    public CSharpCodeWriter WriteXmlCommentException(string exceptionType, string comment) => WriteLine($"/// <exception cref=\"{exceptionType}\">{comment}</exception>");
    
    public CSharpCodeWriter WriteDefaultXmlCommentForParameterName() => WriteXmlCommentParam("parameterName", "The name of the parameter (optional).");

    public CSharpCodeWriter WriteDefaultXmlCommentForMessage(string exceptionType) => WriteXmlCommentParam("message", $"The message that will be injected into the {XmlComment.ToSee(exceptionType)} (optional).");

    public CSharpCodeWriter WriteDefaultArgumentNullException(string parameterName = "parameter") => WriteXmlCommentException("ArgumentNullException", $"Thrown when {XmlComment.ToParamRef(parameterName)} is null.");

    public CSharpCodeWriter WriteReSharperDisablePossibleMultipleEnumeration() => WriteLine("// ReSharper disable PossibleMultipleEnumeration");

    private void WriteIndentationIfNecessary()
    {
        if (!_isCurrentLineEmpty)
            return;
        _isCurrentLineEmpty = false;
        if (_indentationLevel == 0)
            return;
        for (int i = 0; i < _indentationLevel; ++i)
        {
            _writer.Write(_indentationCharacters);
        }
    }
}

public static class XmlComment
{
    public static string ToParamRef(string parameter) => $"<paramref name=\"{parameter}\"/>";
    public static string ToSee(string token) => $"<see cref=\"{token}\"/>";
}