#! "netcoreapp2.0"
#r "nuget: Light.GuardClauses, 3.5.0"

using Light.GuardClauses;

public sealed class CSharpCodeFileWriter
{
    private readonly TextWriter _writer;
    private int _indentationLevel;
    private readonly string _indentationCharacters;
    private bool _isCurrentLineEmpty = true;

    public CSharpCodeFileWriter(TextWriter writer, int initialIndentationLevel = 0, string indentationCharacters = "    ")
    {
        _writer = writer.MustNotBeNull(nameof(writer));
        _indentationLevel = initialIndentationLevel.MustBeGreaterThanOrEqualTo(0, nameof(initialIndentationLevel));
        _indentationCharacters = indentationCharacters.MustNotBeNull(nameof(indentationCharacters));
    }

    public int IndentationLevel => _indentationLevel;

    public string indentationCharacters => _indentationCharacters;

    public CSharpCodeFileWriter IncreaseIndentation()
    {
        ++_indentationLevel;
        return this;
    }

    public CSharpCodeFileWriter DecreaseIndentation()
    {
        _indentationLevel.MustBeGreaterThan(0, exception: () => new InvalidOperationException("The indentation level cannot be less than 0."));

        --_indentationLevel;
        return this;
    }

    public CSharpCodeFileWriter Write(string code)
    {
        WriteIndentationIfNecessary();
        _writer.Write(code);
        return this;
    }

    public CSharpCodeFileWriter WriteLine(string code)
    {
        Write(code);
        _writer.WriteLine();
        _isCurrentLineEmpty = true;
        return this;
    }

    public CSharpCodeFileWriter OpenScopeAndIndent() => WriteLine("{").IncreaseIndentation();

    public CSharpCodeFileWriter CloseScope() => DecreaseIndentation().WriteLine("}");

    public CSharpCodeFileWriter WriteEmptyLine() => WriteLine(string.Empty);

    public CSharpCodeFileWriter IncludeNamespace(string @namespace) => WriteLine($"using {@namespace};");

    public CSharpCodeFileWriter OpenNamespace(string @namespace) => WriteLine($"namespace {@namespace}").OpenScopeAndIndent();

    public CSharpCodeFileWriter OpenPublicStaticPartialClass(string className) => WriteLine($"public static partial class {className}").OpenScopeAndIndent();

    public CSharpCodeFileWriter WriteAggressiveInliningAttribute() => WriteLine("[MethodImpl(MethodImplOptions.AggressiveInlining)]");

    public CSharpCodeFileWriter OpenMember(string memberHeader) => WriteLine(memberHeader).OpenScopeAndIndent();

    public CSharpCodeFileWriter CloseRemainingScopes(bool startOnPreviousIndentationLevel = true)
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

    public CSharpCodeFileWriter WriteCodeGenerationNotice(string generatorName) =>
        WriteLine($@"
// PLEASE NOTICE: this code is auto-generated. Any changes you make to this file are going to be overwritten when this file is recreated by the code generator.
// Check ""{generatorName}"" for details.")
       .WriteEmptyLine();

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