using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using FluentAssertions;
using Light.GuardClauses.FrameworkExtensions;
using Xunit;

namespace Light.GuardClauses.Tests.FrameworkExtensions;

public static class AsReadOnlyListTests
{
    [Theory]
    [MemberData(nameof(ReturnsCastInstanceIfPossibleData))]
    public static void ReturnsCastInstanceIfPossible(IEnumerable<string> enumerable)
    {
        // ReSharper disable PossibleMultipleEnumeration
        var readOnlyList = enumerable.AsReadOnlyList();

        readOnlyList.Should().BeSameAs(enumerable);
        // ReSharper restore PossibleMultipleEnumeration
    }

    [Theory]
    [MemberData(nameof(ReturnsCastInstanceIfPossibleData))]
    public static void ReturnsCastInstanceIfPossibleWithCustomFactory(IEnumerable<string> enumerable)
    {
        // ReSharper disable PossibleMultipleEnumeration
        var readOnlyList = enumerable.AsReadOnlyList(items => new ObservableCollection<string>(items));

        readOnlyList.Should().BeSameAs(enumerable);
        // ReSharper restore PossibleMultipleEnumeration
    }

    public static readonly TheoryData<IEnumerable<string>> ReturnsCastInstanceIfPossibleData =
        new()
        {
            new List<string> { "Foo", "Bar" },
            new[] { "Foo", "Bar", "Baz" },
            new ObservableCollection<string>()
        };

    [Fact]
    public static void ReturnsNewListIfCastNotPossible()
    {
        // ReSharper disable PossibleMultipleEnumeration
        var enumerable = LazyEnumerable();

        var list = enumerable.AsReadOnlyList();

        list.Should().NotBeSameAs(enumerable);
        list.Should().BeOfType<List<string>>();
        list.Should().Equal(enumerable);
        // ReSharper restore PossibleMultipleEnumeration
    }

    [Fact]
    public static void EnumerableNull()
    {
        Action act = () => ((IEnumerable<string>) null)!.AsReadOnlyList();

        act.Should().Throw<ArgumentNullException>()
           .And.ParamName.Should().Be("source");
    }

    [Fact]
    public static void CreateCollection()
    {
        var collection = LazyEnumerable().AsReadOnlyList(items => new ObservableCollection<string>(items));

        collection.Should().BeAssignableTo<ObservableCollection<string>>();
        collection.Should().Equal(LazyEnumerable());
    }

    private static IEnumerable<string> LazyEnumerable()
    {
        yield return "Foo";
        yield return "Bar";
        yield return "Baz";
    }
}