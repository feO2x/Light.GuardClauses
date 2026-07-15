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

public static class MustNotContainKeyTests
{
    [Fact]
    public static void KeyPresent()
    {
        var dictionary = new Dictionary<string, int> { ["Foo"] = 1, ["Bar"] = 2 };

        Action act = () => dictionary.MustNotContainKey("Foo");

        act.Should().Throw<ExistingKeyException>()
           .And.Message.Should()
           .Contain($"{nameof(dictionary)} must not contain key \"Foo\", but it actually does.");
    }

    [Fact]
    public static void KeyNotPresent()
    {
        var dictionary = new Dictionary<int, string> { [42] = "Foo" };

        dictionary.MustNotContainKey(86).Should().BeSameAs(dictionary);
    }

    [Fact]
    public static void InterfaceKeyPresent()
    {
        IReadOnlyDictionary<string, object> dictionary = new Dictionary<string, object> { ["Foo"] = 1 };

        Action act = () => dictionary.MustNotContainKey("Foo");

        act.Should().Throw<ExistingKeyException>()
           .And.Message.Should()
           .Contain($"{nameof(dictionary)} must not contain key \"Foo\", but it actually does.");
    }

    [Fact]
    public static void InterfaceKeyNotPresent()
    {
        IReadOnlyDictionary<string, object> dictionary = new Dictionary<string, object> { ["Foo"] = 1 };

        dictionary.MustNotContainKey("Bar").Should().BeSameAs(dictionary);
    }

    [Fact]
    public static void DictionaryNull()
    {
        Action act = () => ((Dictionary<string, string>) null).MustNotContainKey("Foo");

        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public static void InterfaceDictionaryNull()
    {
        Action act = () => ((IReadOnlyDictionary<string, string>) null).MustNotContainKey("Foo");

        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public static void KeyComparerIsRespected()
    {
        var dictionary = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase) { ["FOO"] = 1 };

        Action act = () => dictionary.MustNotContainKey("foo");

        act.Should().Throw<ExistingKeyException>();
    }

    [Fact]
    public static void OrdinalKeyComparerDoesNotMatchDifferentCasing()
    {
        var dictionary = new Dictionary<string, int>(StringComparer.Ordinal) { ["FOO"] = 1 };

        dictionary.MustNotContainKey("foo").Should().BeSameAs(dictionary);
    }

    [Fact]
    public static void CustomException()
    {
        var dictionary = new Dictionary<string, int> { ["Foo"] = 1 };

        Test.CustomException(
            dictionary,
            "Foo",
            (d, key, exceptionFactory) => d.MustNotContainKey(key, exceptionFactory)
        );
    }

    [Fact]
    public static void CustomExceptionDictionaryNull() =>
        Test.CustomException(
            (Dictionary<string, int>) null,
            "Foo",
            (d, key, exceptionFactory) => d.MustNotContainKey(key, exceptionFactory)
        );

    [Fact]
    public static void InterfaceCustomException()
    {
        IReadOnlyDictionary<string, int> dictionary = new Dictionary<string, int> { ["Foo"] = 1 };

        Test.CustomException(
            dictionary,
            "Foo",
            (d, key, exceptionFactory) => d.MustNotContainKey(key, exceptionFactory)
        );
    }

    [Fact]
    public static void CustomExceptionNotThrown()
    {
        var dictionary = new Dictionary<string, int> { ["Foo"] = 1 };

        dictionary.MustNotContainKey("Bar", (_, _) => new ()).Should().BeSameAs(dictionary);
    }

    [Fact]
    public static void InterfaceCustomExceptionNotThrown()
    {
        IReadOnlyDictionary<string, int> dictionary = new Dictionary<string, int> { ["Foo"] = 1 };

        dictionary.MustNotContainKey("Bar", (_, _) => new ()).Should().BeSameAs(dictionary);
    }

    [Fact]
    public static void CustomMessage() =>
        Test.CustomMessage<ExistingKeyException>(
            message => new Dictionary<string, int> { ["Foo"] = 1 }.MustNotContainKey("Foo", message: message)
        );

    [Fact]
    public static void CustomMessageDictionaryNull() =>
        Test.CustomMessage<ArgumentNullException>(
            message => ((Dictionary<string, int>) null).MustNotContainKey("Foo", message: message)
        );

    [Fact]
    public static void CallerArgumentExpression()
    {
        var settings = new Dictionary<string, string> { ["Foo"] = "Bar" };

        var act = () => settings.MustNotContainKey("Foo");

        act.Should().Throw<ExistingKeyException>()
           .WithParameterName(nameof(settings));
    }

    [Fact]
    public static void DictionaryShapeIsPreservedInFluentChains()
    {
        var map = new Dictionary<string, string> { ["endpoint"] = "https://example.com" };

        var result = map.MustNotBeNull().MustNotContainKey("proxy");

        result.Should().BeSameAs(map);
    }

    [Fact]
    public static void ConcurrentDictionaryBindsToInterfaceOverload()
    {
        var dictionary = new ConcurrentDictionary<string, int>();
        dictionary.TryAdd("Foo", 1).Should().BeTrue();

        dictionary.MustNotContainKey("Bar").Should().BeSameAs(dictionary);
    }

    [Fact]
    public static void SortedDictionaryBindsToInterfaceOverload()
    {
        var dictionary = new SortedDictionary<string, int> { ["Foo"] = 1 };

        dictionary.MustNotContainKey("Bar").Should().BeSameAs(dictionary);
    }

    [Fact]
    public static void ReadOnlyDictionaryBindsToInterfaceOverload()
    {
        var dictionary = new ReadOnlyDictionary<string, int>(new Dictionary<string, int> { ["Foo"] = 1 });

        dictionary.MustNotContainKey("Bar").Should().BeSameAs(dictionary);
    }

    [Fact]
    public static void ImmutableDictionaryBindsToInterfaceOverload()
    {
        var dictionary = ImmutableDictionary<string, int>.Empty.Add("Foo", 1);

        dictionary.MustNotContainKey("Bar").Should().BeSameAs(dictionary);
    }

#if NET8_0_OR_GREATER
    [Fact]
    public static void FrozenDictionaryBindsToInterfaceOverload()
    {
        var dictionary = new Dictionary<string, int> { ["Foo"] = 1 }.ToFrozenDictionary();

        dictionary.MustNotContainKey("Bar").Should().BeSameAs(dictionary);
    }
#endif
}
