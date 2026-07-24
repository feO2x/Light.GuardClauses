#nullable enable

using System;
using System.Collections.Generic;
using System.IO;
using FluentAssertions;
using Xunit;

namespace Light.GuardClauses.Tests.TypeAssertions;

public static class MustBeConcreteClassTests
{
    [Theory(DisplayName = "MustBeConcreteClass must return types accepted by IsClass && !IsAbstract.")]
    [MemberData(nameof(ConcreteClassTypes))]
    public static void ConcreteClass(Type type)
    {
        var result = type.MustBeConcreteClass();

        result.Should().BeSameAs(type);
    }

    public static readonly TheoryData<Type> ConcreteClassTypes =
        new ()
        {
            typeof(SampleConcreteClass),
            typeof(SealedConcreteClass),
            typeof(SampleDelegate),
            typeof(string[]),
            typeof(List<>),
            typeof(List<string>),
        };

    [Theory(DisplayName = "MustBeConcreteClass must reject types not accepted by IsClass && !IsAbstract.")]
    [MemberData(nameof(NonConcreteClassTypes))]
    public static void NonConcreteClass(Type type)
    {
        Action act = () => type.MustBeConcreteClass();

        var exception = act.Should().Throw<ArgumentException>().Which;
        exception.ParamName.Should().Be(nameof(type));
        exception.Message.Should().Contain($"\"{type}\"");
        exception.Message.Should().Contain("non-abstract class");
    }

    public static readonly TheoryData<Type> NonConcreteClassTypes =
        new ()
        {
            typeof(IDisposable),
            typeof(AbstractClass),
            typeof(StaticClass),
            typeof(int),
            typeof(DayOfWeek),
            typeof(void),
        };

    [Fact]
    public static void NullType()
    {
        Type? type = null;

        Action act = () => type.MustBeConcreteClass();

        act.Should().Throw<ArgumentNullException>()
           .WithParameterName(nameof(type));
    }

    [Fact]
    public static void ExplicitParameterName()
    {
        Action act = () => typeof(IDisposable).MustBeConcreteClass("handlerType");

        act.Should().Throw<ArgumentException>()
           .WithParameterName("handlerType");
    }

    [Fact]
    public static void CustomMessage() =>
        Test.CustomMessage<ArgumentException>(
            message => typeof(IDisposable).MustBeConcreteClass(message: message)
        );

    [Theory]
    [MemberData(nameof(FactoryFailureData))]
    public static void FactoryReceivesOriginalTypeAndItsExceptionIsThrown(Type? type, Type? expectedFactoryArgument)
    {
        var expectedException = new InvalidOperationException("Custom concrete-class failure");
        Type? capturedType = typeof(void);

        Exception ExceptionFactory(Type? originalType)
        {
            capturedType = originalType;
            return expectedException;
        }

        Action act = () => type.MustBeConcreteClass(ExceptionFactory);

        act.Should().Throw<InvalidOperationException>()
           .Which.Should().BeSameAs(expectedException);
        capturedType.Should().BeSameAs(expectedFactoryArgument);
    }

    public static readonly TheoryData<Type?, Type?> FactoryFailureData =
        new ()
        {
            { typeof(IDisposable), typeof(IDisposable) },
            { typeof(AbstractClass), typeof(AbstractClass) },
            { null, null },
        };

    [Fact]
    public static void FactoryIsNotInvokedOnSuccess()
    {
        var type = typeof(SampleConcreteClass);
        var wasInvoked = false;

        var result = type.MustBeConcreteClass(
            _ =>
            {
                wasInvoked = true;
                return new Exception();
            }
        );

        result.Should().BeSameAs(type);
        wasInvoked.Should().BeFalse();
    }

    [Fact]
    public static void NullFactoryIsIgnoredOnSuccess()
    {
        var type = typeof(SampleConcreteClass);

        var result = type.MustBeConcreteClass((Func<Type?, Exception>) null!);

        result.Should().BeSameAs(type);
    }

    [Fact]
    public static void NullFactoryThrowsArgumentNullExceptionOnFailure()
    {
        Action act = () => typeof(IDisposable).MustBeConcreteClass((Func<Type?, Exception>) null!);

        act.Should().Throw<ArgumentNullException>()
           .WithParameterName("exceptionFactory");
    }

    [Fact]
    public static void ComposesWithMustBeAssignableTo()
    {
        var type = typeof(MemoryStream);

        var result = type.MustBeConcreteClass()
                         .MustBeAssignableTo(typeof(Stream));

        result.Should().BeSameAs(type);
    }

    private class SampleConcreteClass;

    private sealed class SealedConcreteClass;

    private abstract class AbstractClass;

    private static class StaticClass;

    private delegate void SampleDelegate();
}
