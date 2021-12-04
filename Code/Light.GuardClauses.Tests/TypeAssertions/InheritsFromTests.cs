using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using FluentAssertions;
using Xunit;

namespace Light.GuardClauses.Tests.TypeAssertions;

public static class InheritsFromTests
{
    [Fact]
    public static void BaseClasses()
    {
        CheckInheritsFrom(typeof(ArgumentException), typeof(Exception), true);
        CheckInheritsFrom(typeof(ArgumentNullException), typeof(Exception), true);
        CheckInheritsFrom(typeof(List<>), typeof(Encoding), false);
        CheckInheritsFrom(typeof(ObservableCollection<object>), typeof(Collection<>), true);
        CheckInheritsFrom(typeof(double), typeof(ValueType), true);
    }

    [Fact]
    public static void Interfaces()
    {
        CheckInheritsFrom(typeof(string), typeof(IComparable), true);
        CheckInheritsFrom(typeof(string), typeof(IEnumerable<char>), true);
        CheckInheritsFrom(typeof(string), typeof(IEnumerable<>), true);
        CheckInheritsFrom(typeof(string), typeof(IEqualityComparer<>), false);

        CheckInheritsFrom(typeof(int), typeof(IEquatable<int>), true);
        CheckInheritsFrom(typeof(int), typeof(IServiceProvider), false);

        CheckInheritsFrom(typeof(IList<object>), typeof(ICollection<object>), true);
        CheckInheritsFrom(typeof(IList<object>), typeof(ICollection<>), true);
        CheckInheritsFrom(typeof(IList<>), typeof(IDictionary<,>), false);
    }

    [Fact]
    public static void OtherTypes()
    {
        CheckInheritsFrom(typeof(int), typeof(ConsoleColor), false);
        CheckInheritsFrom(typeof(string), typeof(double), false);
        CheckInheritsFrom(typeof(UnicodeEncoding), typeof(Func<>), false);
    }

    [Fact]
    public static void TypeNull()
    {
        var type = default(Type);

        // ReSharper disable once ExpressionIsAlwaysNull
        Action act = () => type.InheritsFrom(typeof(object));

        act.Should().Throw<ArgumentNullException>()
           .And.ParamName.Should().Be(nameof(type));
    }

    [Fact(DisplayName = "IsDerivingFromOrImplementing must throw an ArgumentNullException when the specified base type is null.")]
    public static void BaseTypeNull()
    {
        var baseClassOrInterfaceType = default(Type);

        // ReSharper disable once ExpressionIsAlwaysNull
        Action act = () => typeof(string).InheritsFrom(baseClassOrInterfaceType);

        act.Should().Throw<ArgumentNullException>()
           .And.ParamName.Should().Be(nameof(baseClassOrInterfaceType));
    }

    private static void CheckInheritsFrom(Type type, Type baseClassOrInterfaceType, bool expected)
    {
        type.InheritsFrom(baseClassOrInterfaceType).Should().Be(expected);
    }
}