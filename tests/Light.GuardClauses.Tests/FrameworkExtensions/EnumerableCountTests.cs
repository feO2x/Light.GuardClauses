using System.Collections;
using System.Collections.Generic;
using FluentAssertions;
using Light.GuardClauses.FrameworkExtensions;
using Xunit;

namespace Light.GuardClauses.Tests.FrameworkExtensions;

public static class EnumerableCountTests
{
    [Fact]
    public static void CountWithExceptionDetailsUsesStringLengthWithoutEnumerating()
    {
        IEnumerable value = "text";

        value.Count("value", "message").Should().Be(4);
    }

    [Fact]
    public static void GenericCountWithExceptionDetailsUsesEveryAvailableStrategy()
    {
        IEnumerable<char> text = "text";
        var collection = new HashSet<int> { 1, 2, 3 };
        var readOnlyCollection = new ReadOnlyCollectionOnly<int>([1, 2, 3, 4]);

        text.GetCount("text", "message").Should().Be(4);
        collection.GetCount("collection", "message").Should().Be(3);
        readOnlyCollection.GetCount("readOnlyCollection", "message").Should().Be(4);
        Yield(1, 2, 3, 4, 5).GetCount("items", "message").Should().Be(5);
    }

    [Fact]
    public static void IsOneOfEnumeratesSourcesThatAreNotCollections()
    {
        2.IsOneOf(Yield(1, 2, 3)).Should().BeTrue();
        4.IsOneOf(Yield(1, 2, 3)).Should().BeFalse();
    }

    private static IEnumerable<T> Yield<T>(params T[] items)
    {
        foreach (var item in items)
        {
            yield return item;
        }
    }

    private sealed class ReadOnlyCollectionOnly<T>(IReadOnlyCollection<T> items) : IReadOnlyCollection<T>
    {
        public int Count => items.Count;

        public IEnumerator<T> GetEnumerator() => items.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
