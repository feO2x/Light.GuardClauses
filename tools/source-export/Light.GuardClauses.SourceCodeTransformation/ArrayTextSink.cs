using System;

namespace Light.GuardClauses.SourceCodeTransformation;

internal struct ArrayTextSink
{
    private readonly char[] _internalArray;
    private int _currentIndex;

    public ArrayTextSink(char[] internalArray)
    {
        _internalArray = internalArray.MustNotBeNull(nameof(internalArray));
        _currentIndex = 0;
    }

    public void Append(ReadOnlySpan<char> text)
    {
        for (var i = 0; i < text.Length; ++i)
            _internalArray[_currentIndex++] = text[i];
    }

    public Memory<char> ToMemory() => new Memory<char>(_internalArray, 0, _currentIndex);
}