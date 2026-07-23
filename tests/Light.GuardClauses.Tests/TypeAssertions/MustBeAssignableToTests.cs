#nullable enable

using System;
using System.Collections.Generic;
using FluentAssertions;
using Xunit;

namespace Light.GuardClauses.Tests.TypeAssertions;

public static class MustBeAssignableToTests
{
    [Theory(DisplayName = "MustBeAssignableTo must return the candidate type for valid CLR assignability relations.")]
    [MemberData(nameof(AssignableTypes))]
    public static void Assignable(Type candidateType, Type requiredType)
    {
        var result = candidateType.MustBeAssignableTo(requiredType);

        result.Should().BeSameAs(candidateType);
    }

    public static readonly TheoryData<Type, Type> AssignableTypes =
        new ()
        {
            { typeof(string), typeof(string) },
            { typeof(ArgumentNullException), typeof(ArgumentException) },
            { typeof(ArgumentNullException), typeof(Exception) },
            { typeof(List<string>), typeof(IEnumerable<string>) },
            { typeof(int), typeof(ValueType) },
            { typeof(IEnumerable<string>), typeof(IEnumerable<object>) },
            { typeof(IComparer<object>), typeof(IComparer<string>) },
            { typeof(string[]), typeof(object[]) },
            { typeof(IEnumerable<>), typeof(IEnumerable<>) },
        };

    [Theory(DisplayName = "MustBeAssignableTo must follow CLR failures, including reversed and open-generic relations.")]
    [MemberData(nameof(UnassignableTypes))]
    public static void Unassignable(Type candidateType, Type requiredType)
    {
        Action act = () => candidateType.MustBeAssignableTo(requiredType);

        var exception = act.Should().Throw<ArgumentException>().Which;
        exception.ParamName.Should().Be(nameof(candidateType));
        exception.Message.Should().Contain($"\"{candidateType}\"");
        exception.Message.Should().Contain($"\"{requiredType}\"");
        exception.Message.Should().Contain("assignable to");
    }

    public static readonly TheoryData<Type, Type> UnassignableTypes =
        new ()
        {
            { typeof(Exception), typeof(ArgumentException) },
            { typeof(string), typeof(IDisposable) },
            { typeof(int), typeof(long) },
            { typeof(int[]), typeof(object[]) },
            { typeof(List<>), typeof(IEnumerable<>) },
            { typeof(List<string>), typeof(IEnumerable<>) },
        };

    [Fact]
    public static void NullCandidateType()
    {
        Type? candidateType = null;

        Action act = () => candidateType.MustBeAssignableTo(typeof(object));

        act.Should().Throw<ArgumentNullException>()
           .WithParameterName(nameof(candidateType));
    }

    [Fact]
    public static void NullRequiredType()
    {
        Type? requiredType = null;

        Action act = () => typeof(string).MustBeAssignableTo(requiredType);

        act.Should().Throw<ArgumentNullException>()
           .WithParameterName(nameof(requiredType));
    }

    [Fact]
    public static void ExplicitParameterName()
    {
        Action act = () => typeof(object).MustBeAssignableTo(typeof(string), "serviceType");

        act.Should().Throw<ArgumentException>()
           .WithParameterName("serviceType");
    }

    [Fact]
    public static void CustomMessage() =>
        Test.CustomMessage<ArgumentException>(
            message => typeof(object).MustBeAssignableTo(typeof(string), message: message)
        );

    [Theory]
    [MemberData(nameof(FactoryFailureData))]
    public static void FactoryReceivesOriginalTypesAndItsExceptionIsThrown(Type? candidateType, Type? requiredType)
    {
        var expectedException = new InvalidOperationException("Custom assignability failure");
        Type? capturedCandidate = typeof(void);
        Type? capturedRequired = typeof(void);

        Exception ExceptionFactory(Type? candidate, Type? required)
        {
            capturedCandidate = candidate;
            capturedRequired = required;
            return expectedException;
        }

        Action act = () => candidateType.MustBeAssignableTo(requiredType, ExceptionFactory);

        act.Should().Throw<InvalidOperationException>()
           .Which.Should().BeSameAs(expectedException);
        capturedCandidate.Should().BeSameAs(candidateType);
        capturedRequired.Should().BeSameAs(requiredType);
    }

    public static readonly TheoryData<Type?, Type?> FactoryFailureData =
        new ()
        {
            { typeof(string), typeof(IDisposable) },
            { null, typeof(object) },
            { typeof(string), null },
        };

    [Fact]
    public static void FactoryIsNotInvokedOnSuccess()
    {
        var candidateType = typeof(string);
        var wasInvoked = false;

        var result = candidateType.MustBeAssignableTo(
            typeof(object),
            (_, _) =>
            {
                wasInvoked = true;
                return new Exception();
            }
        );

        result.Should().BeSameAs(candidateType);
        wasInvoked.Should().BeFalse();
    }

    [Fact]
    public static void NullFactoryIsIgnoredOnSuccess()
    {
        var candidateType = typeof(string);

        var result = candidateType.MustBeAssignableTo(
            typeof(object),
            (Func<Type?, Type?, Exception>) null!
        );

        result.Should().BeSameAs(candidateType);
    }

    [Fact]
    public static void NullFactoryThrowsArgumentNullExceptionOnFailure()
    {
        Action act = () => typeof(object).MustBeAssignableTo(
            typeof(string),
            (Func<Type?, Type?, Exception>) null!
        );

        act.Should().Throw<ArgumentNullException>()
           .WithParameterName("exceptionFactory");
    }
}
