using System;

namespace Light.GuardClauses.SourceCodeTransformation;

public static class CleanupStep
{
    public static Memory<char> Cleanup(string sourceCode, SourceFileMergeOptions options)
    {
        if (options.RemoveCallerArgumentExpressions)
            sourceCode = sourceCode.Replace("[CallerArgumentExpression(\"parameter\")] ", string.Empty);
        if (options.RemoveValidatedNotNull)
            sourceCode = sourceCode.Replace("[ValidatedNotNull] ", string.Empty);
        if (options.RemoveNotNullWhen)
        {
            sourceCode = sourceCode.Replace("[NotNullWhen(true)] ", string.Empty);
            sourceCode = sourceCode.Replace("[NotNullWhen(false)] ", string.Empty);
        }

        var sink = new ArrayTextSink(new char[sourceCode.Length]);
        var parser = new LineOfCodeParser(sourceCode);

        LineOfCode previousLineOfCode = default;
        while (parser.TryGetNext(out var currentLineOfCode))
        {
            var isEmptyLineAfterXmlComment = previousLineOfCode.Type == LineOfCodeType.XmlComment && currentLineOfCode.Type == LineOfCodeType.WhiteSpace;
            var isContractAnnotationThatShouldBeRemoved = options.RemoveContractAnnotations && currentLineOfCode.Type == LineOfCodeType.ContractAnnotation;
            var isDoesNotReturnThatShouldBeRemoved = options.RemoveDoesNotReturn && currentLineOfCode.Type == LineOfCodeType.DoesNotReturnAnnotation;

            if (isEmptyLineAfterXmlComment)
            {
                // Break point parking spot
            }

            if (!isEmptyLineAfterXmlComment && !isContractAnnotationThatShouldBeRemoved && !isDoesNotReturnThatShouldBeRemoved)
                sink.Append(currentLineOfCode.Span);

            previousLineOfCode = currentLineOfCode;
        }

        return sink.ToMemory();
    }

    private ref struct LineOfCodeParser
    {
        private readonly ReadOnlySpan<char> _sourceCode;
        private int _currentIndex;

        public LineOfCodeParser(ReadOnlySpan<char> sourceCode)
        {
            _sourceCode = sourceCode;
            _currentIndex = 0;
        }

        public bool TryGetNext(out LineOfCode lineOfCode)
        {
            if (!_sourceCode.TryGetNextLine(_currentIndex, out var currentLineSpan))
            {
                lineOfCode = default;
                return false;
            }

            _currentIndex += currentLineSpan.Length;

            var leftTrimmedSpan = currentLineSpan.TrimStart();
            if (leftTrimmedSpan.StartsWith("///"))
                lineOfCode = new LineOfCode(LineOfCodeType.XmlComment, currentLineSpan);
            else if (leftTrimmedSpan.StartsWith("[ContractAnnotation("))
                lineOfCode = new LineOfCode(LineOfCodeType.ContractAnnotation, currentLineSpan);
            else if (leftTrimmedSpan.StartsWith("[DoesNotReturn]"))
                lineOfCode = new LineOfCode(LineOfCodeType.DoesNotReturnAnnotation, currentLineSpan);
            else if (leftTrimmedSpan.IsEmpty)
                lineOfCode = new LineOfCode(LineOfCodeType.WhiteSpace, currentLineSpan);
            else
                lineOfCode = new LineOfCode(LineOfCodeType.Other, currentLineSpan);

            return true;

        }
    }

    private readonly ref struct LineOfCode
    {
        public readonly LineOfCodeType Type;
        public readonly ReadOnlySpan<char> Span;

        public LineOfCode(LineOfCodeType type, ReadOnlySpan<char> span)
        {
            Type = type;
            Span = span;
        }

        public override string ToString() => Span.ToString();
    }

    private enum LineOfCodeType
    {
        Other,
        XmlComment,
        WhiteSpace,
        ContractAnnotation,
        DoesNotReturnAnnotation
    }

    public static bool TryGetNextLine(this in ReadOnlySpan<char> text, int startIndex, out ReadOnlySpan<char> nextLine)
    {
        if (startIndex >= text.Length)
        {
            nextLine = default;
            return false;
        }

        nextLine = text.Slice(startIndex);
        var newLineIndex = nextLine.IndexOf(Environment.NewLine);
        if (newLineIndex == -1)
            return true;

        nextLine = nextLine.Slice(0, newLineIndex + Environment.NewLine.Length);
        return true;
    }
}