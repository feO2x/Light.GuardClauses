using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Xunit;
#if NET8_0_OR_GREATER
using System.Collections.Frozen;
#endif

namespace Light.GuardClauses.Tests.CollectionAssertions;

public static class MustContainKeyTests
{
    [Fact]
    public static void KeyNotPresent()
    {
        var dictionary = new Dictionary<string, int> { ["Foo"] = 1, ["Bar"] = 2 };

        Action act = () => dictionary.MustContainKey("Baz");

        var assertion = act.Should().Throw<MissingKeyException>().Which;
        assertion.Message.Should()
                 .Contain($"{nameof(dictionary)} must contain key \"Baz\", but it actually does not.");
        assertion.Message.Should().Contain("Keys of the dictionary:");
        assertion.Message.Should().Contain("\"Foo\"");
    }

    [Fact]
    public static void KeyPresent()
    {
        var dictionary = new Dictionary<int, string> { [42] = "Foo", [86] = "Bar" };

        dictionary.MustContainKey(42).Should().BeSameAs(dictionary);
    }

    [Fact]
    public static void InterfaceKeyNotPresent()
    {
        IReadOnlyDictionary<string, object> dictionary = new Dictionary<string, object> { ["Foo"] = 1 };

        Action act = () => dictionary.MustContainKey("Bar");

        act.Should().Throw<MissingKeyException>()
           .And.Message.Should()
           .Contain($"{nameof(dictionary)} must contain key \"Bar\", but it actually does not.");
    }

    [Fact]
    public static void InterfaceKeyPresent()
    {
        IReadOnlyDictionary<string, object> dictionary = new Dictionary<string, object> { ["Foo"] = 1 };

        dictionary.MustContainKey("Foo").Should().BeSameAs(dictionary);
    }

    [Fact]
    public static void DictionaryNull()
    {
        Action act = () => ((Dictionary<string, string>) null).MustContainKey("Foo");

        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public static void InterfaceDictionaryNull()
    {
        Action act = () => ((IReadOnlyDictionary<string, string>) null).MustContainKey("Foo");

        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public static void KeyComparerIsRespected()
    {
        var dictionary = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase) { ["FOO"] = 1 };

        dictionary.MustContainKey("foo").Should().BeSameAs(dictionary);
    }

    [Fact]
    public static void OrdinalKeyComparerDoesNotMatchDifferentCasing()
    {
        var dictionary = new Dictionary<string, int>(StringComparer.Ordinal) { ["FOO"] = 1 };

        Action act = () => dictionary.MustContainKey("foo");

        act.Should().Throw<MissingKeyException>();
    }

    [Fact]
    public static void CustomException()
    {
        var dictionary = new Dictionary<string, int> { ["Foo"] = 1 };

        Test.CustomException(
            dictionary,
            "Bar",
            (d, key, exceptionFactory) => d.MustContainKey(key, exceptionFactory)
        );
    }

    [Fact]
    public static void CustomExceptionDictionaryNull() =>
        Test.CustomException(
            (Dictionary<string, int>) null,
            "Foo",
            (d, key, exceptionFactory) => d.MustContainKey(key, exceptionFactory)
        );

    [Fact]
    public static void InterfaceCustomException()
    {
        IReadOnlyDictionary<string, int> dictionary = new Dictionary<string, int> { ["Foo"] = 1 };

        Test.CustomException(
            dictionary,
            "Bar",
            (d, key, exceptionFactory) => d.MustContainKey(key, exceptionFactory)
        );
    }

    [Fact]
    public static void CustomExceptionNotThrown()
    {
        var dictionary = new Dictionary<string, int> { ["Foo"] = 1 };

        dictionary.MustContainKey("Foo", (_, _) => new ()).Should().BeSameAs(dictionary);
    }

    [Fact]
    public static void InterfaceCustomExceptionNotThrown()
    {
        IReadOnlyDictionary<string, int> dictionary = new Dictionary<string, int> { ["Foo"] = 1 };

        dictionary.MustContainKey("Foo", (_, _) => new ()).Should().BeSameAs(dictionary);
    }

    [Fact]
    public static void CustomMessage() =>
        Test.CustomMessage<MissingKeyException>(
            message => new Dictionary<string, int>().MustContainKey("Foo", message: message)
        );

    [Fact]
    public static void CustomMessageDictionaryNull() =>
        Test.CustomMessage<ArgumentNullException>(
            message => ((Dictionary<string, int>) null).MustContainKey("Foo", message: message)
        );

    [Fact]
    public static void CallerArgumentExpression()
    {
        var settings = new Dictionary<string, string> { ["Foo"] = "Bar" };

        var act = () => settings.MustContainKey("Baz");

        act.Should().Throw<MissingKeyException>()
           .WithParameterName(nameof(settings));
    }

    [Fact]
    public static void DictionaryShapeIsPreservedInFluentChains()
    {
        var map = new Dictionary<string, string> { ["endpoint"] = "https://example.com" };

        var result = map.MustNotBeNull().MustContainKey("endpoint");

        result.Should().BeSameAs(map);
    }

    [Fact]
    public static void ConcurrentDictionaryBindsToInterfaceOverload()
    {
        var dictionary = new ConcurrentDictionary<string, int>();
        dictionary.TryAdd("Foo", 1).Should().BeTrue();

        dictionary.MustContainKey("Foo").Should().BeSameAs(dictionary);
    }

    [Fact]
    public static void SortedDictionaryBindsToInterfaceOverload()
    {
        var dictionary = new SortedDictionary<string, int> { ["Foo"] = 1 };

        dictionary.MustContainKey("Foo").Should().BeSameAs(dictionary);
    }

    [Fact]
    public static void ReadOnlyDictionaryBindsToInterfaceOverload()
    {
        var dictionary = new ReadOnlyDictionary<string, int>(new Dictionary<string, int> { ["Foo"] = 1 });

        dictionary.MustContainKey("Foo").Should().BeSameAs(dictionary);
    }

    [Fact]
    public static void ImmutableDictionaryBindsToInterfaceOverload()
    {
        var dictionary = ImmutableDictionary<string, int>.Empty.Add("Foo", 1);

        dictionary.MustContainKey("Foo").Should().BeSameAs(dictionary);
    }

#if NET8_0_OR_GREATER
    [Fact]
    public static void FrozenDictionaryBindsToInterfaceOverload()
    {
        var dictionary = new Dictionary<string, int> { ["Foo"] = 1 }.ToFrozenDictionary();

        dictionary.MustContainKey("Foo").Should().BeSameAs(dictionary);
    }
#endif
}
