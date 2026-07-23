using System;
using FluentAssertions;
using Xunit;

namespace Light.GuardClauses.Tests.CommonAssertions;

public static class ObjectDisposedTests
{
    [Theory]
    [DefaultVariablesData]
    public static void ConditionTrue(string objectName)
    {
        var act = () => Check.ObjectDisposed(true, objectName);

        var exceptionAssertion = act.Should().Throw<ObjectDisposedException>().And;
        exceptionAssertion.ObjectName.Should().BeSameAs(objectName);
        exceptionAssertion.Message.Should().Contain($"Object name: '{objectName}'.");
    }

    [Theory]
    [DefaultVariablesData]
    public static void CustomMessagePropagated(string objectName)
    {
        const string message = "The connection has already been closed.";

        var act = () => Check.ObjectDisposed(true, objectName, message);

        var exceptionAssertion = act.Should().Throw<ObjectDisposedException>().And;
        exceptionAssertion.ObjectName.Should().BeSameAs(objectName);
        exceptionAssertion.Message.Should().Contain(message);
        exceptionAssertion.Message.Should().Contain($"Object name: '{objectName}'.");
    }

    [Fact]
    public static void ConditionFalse()
    {
        Check.ObjectDisposed(false);
    }

    [Fact]
    public static void ConditionFalseCustomException()
    {
        Check.ObjectDisposed(false, () => new Exception());
    }

    [Fact]
    public static void ConditionFalseCustomExceptionWithParameter()
    {
        Check.ObjectDisposed(false, new object(), _ => new Exception());
    }

    [Fact]
    public static void CustomException() =>
        Test.CustomException(exceptionFactory => Check.ObjectDisposed(true, exceptionFactory));

    [Fact]
    public static void CustomExceptionWithParameter() =>
        Test.CustomException(new object(),
                             (parameter, exceptionFactory) => Check.ObjectDisposed(true, parameter, exceptionFactory));

    [Fact]
    public static void NullFactory()
    {
        var act = () => Check.ObjectDisposed(true, (Func<Exception>) null!);

        act.Should().Throw<ArgumentNullException>()
           .And.ParamName.Should().Be("exceptionFactory");
    }

    [Fact]
    public static void NullFactoryWithParameter()
    {
        var act = () => Check.ObjectDisposed(true, new object(), (Func<object, Exception>) null!);

        act.Should().Throw<ArgumentNullException>()
           .And.ParamName.Should().Be("exceptionFactory");
    }

    [Fact]
    public static void DefaultMessage()
    {
        var act = () => Check.ObjectDisposed(true);

        act.Should().Throw<ObjectDisposedException>()
           .And.Message.Should().Be("Cannot access a disposed object.");
    }
}
