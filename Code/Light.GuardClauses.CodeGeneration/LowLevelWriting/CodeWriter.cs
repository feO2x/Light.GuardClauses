using System;
using System.IO;

namespace Light.GuardClauses.CodeGeneration.LowLevelWriting
{
    public sealed class CodeWriter
    {
        private readonly string _indentationCharacters;
        private readonly TextWriter _writer;
        private uint _indentationLevel;
        private bool _isCurrentLineEmpty = true;

        public CodeWriter(TextWriter writer, uint initialIndentationLevel = 0, string indentationCharacters = "    ")
        {
            _writer = writer.MustNotBeNull(nameof(writer));
            _indentationLevel = initialIndentationLevel.MustBeLessThan(uint.MaxValue, nameof(initialIndentationLevel));
            _indentationCharacters = indentationCharacters.MustNotBeNull(nameof(indentationCharacters));
        }

        public uint IndentationLevel => _indentationLevel;

        public CodeWriter IncreaseIndentation()
        {
            (++_indentationLevel).MustBeLessThan(uint.MaxValue, exception: () => new InvalidOperationException($"The indentation level must not reach {uint.MaxValue}."));
            return this;
        }

        public CodeWriter DecreaseIndentation()
        {
            _indentationLevel.MustBeGreaterThan(0u, exception: () => new InvalidOperationException("The indentation level cannot be less than zero"));

            --_indentationLevel;
            return this;
        }

        public CodeWriter Write(string code)
        {
            WriteIndentationLevelIfNecessary();
            _writer.Write(code);
            return this;
        }

        public CodeWriter WriteLine(string code)
        {
            Write(code);
            _writer.WriteLine();
            _isCurrentLineEmpty = true;
            return this;
        }

        public CodeWriter WriteEmptyLine()
        {
            if (_isCurrentLineEmpty == false)
                WriteLine(string.Empty);
            WriteLine(string.Empty);
            return this;
        }

        private void WriteIndentationLevelIfNecessary()
        {
            if (!_isCurrentLineEmpty)
                return;

            _isCurrentLineEmpty = false;
            if (_indentationLevel == 0)
                return;

            for (var i = 0; i < _indentationLevel; i++)
            {
                _writer.Write(_indentationCharacters);
            }
        }
    }
}