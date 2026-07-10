#nullable enable

using System;
using FluentAssertions;
using FluentAssertions.Specialized;
using Light.GuardClauses.ExceptionFactory;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Light.GuardClauses.Tests;

public static class Test
{
    private static readonly ExceptionDummy Exception = new ();
    private static readonly Func<Exception> ExceptionFactory = () => Exception;

    public static void CustomException(Action<Func<Exception>> executeAssertion)
    {
        try
        {
            executeAssertion(ExceptionFactory);
            throw new XunitException("The assertion should have thrown a custom exception at this point.");
        }
        catch (ExceptionDummy exception)
        {
            exception.Should().BeSameAs(Exception);
        }
    }

    public static void CustomException<T>(T invalidValue, Action<T, Func<T, Exception>> executeAssertion)
    {
        T? capturedParameter = default;

        Exception ExceptionFactory(T parameter)
        {
            capturedParameter = parameter;
            return Exception;
        }

        try
        {
            executeAssertion(invalidValue, ExceptionFactory);
            throw new XunitException("The assertion should have thrown a custom exception at this point.");
        }
        catch (ExceptionDummy exception)
        {
            exception.Should().BeSameAs(Exception);
            capturedParameter.Should().Be(invalidValue);
        }
    }

    public static void CustomSpanException<T>(
        ReadOnlySpan<T> invalidValue,
        ExecuteReadOnlySpanAssertion<T> executeAssertion
    )
    {
        T[]? capturedParameter = null;

        Exception ExceptionFactory(ReadOnlySpan<T> parameter)
        {
            capturedParameter = parameter.ToArray();
            return Exception;
        }

        try
        {
            executeAssertion(invalidValue, ExceptionFactory);
            throw new XunitException("The assertion should have thrown a custom exception at this point.");
        }
        catch (ExceptionDummy exception)
        {
            exception.Should().BeSameAs(exception);
            capturedParameter.Should().Equal(invalidValue.ToArray());
        }
    }

    public static void CustomSpanException<T>(Span<T> invalidValue, ExecuteSpanAssertion<T> executeAssertion)
    {
        T[]? capturedParameter = null;

        Exception ExceptionFactory(ReadOnlySpan<T> parameter)
        {
            capturedParameter = parameter.ToArray();
            return Exception;
        }

        try
        {
            executeAssertion(invalidValue, ExceptionFactory);
            throw new XunitException("The assertion should have thrown a custom exception at this point.");
        }
        catch (ExceptionDummy exception)
        {
            exception.Should().BeSameAs(exception);
            capturedParameter.Should().Equal(invalidValue.ToArray());
        }
    }

    public static void CustomMemoryException<T>(
        Memory<T> invalidValue,
        Action<Memory<T>, ReadOnlySpanExceptionFactory<T>> executeAssertion
    )
    {
        Memory<T> capturedParameter = default;

        Exception ExceptionFactory(ReadOnlySpan<T> parameter)
        {
            capturedParameter = parameter.ToArray();
            return Exception;
        }

        try
        {
            executeAssertion(invalidValue, ExceptionFactory);
            throw new XunitException("The assertion should have thrown a custom exception at this point.");
        }
        catch (ExceptionDummy exception)
        {
            exception.Should().BeSameAs(exception);
            capturedParameter.ToArray().Should().Equal(invalidValue.ToArray());
        }
    }

    public static void CustomMemoryException<T>(
        ReadOnlyMemory<T> invalidValue,
        Action<ReadOnlyMemory<T>, ReadOnlySpanExceptionFactory<T>> executeAssertion
    )
    {
        ReadOnlyMemory<T> capturedParameter = default;

        Exception ExceptionFactory(ReadOnlySpan<T> parameter)
        {
            capturedParameter = parameter.ToArray();
            return Exception;
        }

        try
        {
            executeAssertion(invalidValue, ExceptionFactory);
            throw new XunitException("The assertion should have thrown a custom exception at this point.");
        }
        catch (ExceptionDummy exception)
        {
            exception.Should().BeSameAs(exception);
            capturedParameter.ToArray().Should().Equal(invalidValue.ToArray());
        }
    }

    public static void CustomException<T1, T2>(
        T1 first,
        T2 second,
        Action<T1, T2, Func<T1, T2, Exception>> executeAssertion
    )
    {
        T1? capturedFirst = default;
        T2? capturedSecond = default;

        Exception ExceptionFactory(T1 x, T2 y)
        {
            capturedFirst = x;
            capturedSecond = y;
            return Exception;
        }

        try
        {
            executeAssertion(first, second, ExceptionFactory);
            throw new XunitException("The assertion should have thrown a custom exception at this point.");
        }
        catch (ExceptionDummy exception)
        {
            exception.Should().BeSameAs(Exception);
            capturedFirst.Should().Be(first);
            capturedSecond.Should().Be(second);
        }
    }

    public static void CustomSpanException<T1, T2>(
        ReadOnlySpan<T1> invalidValue,
        T2 additionalValue,
        ExecuteReadOnlySpanAssertion<T1, T2> executeAssertion
    )
    {
        T1[]? capturedFirst = null;
        T2? capturedSecond = default;

        Exception ExceptionFactory(ReadOnlySpan<T1> parameter, T2 second)
        {
            capturedFirst = parameter.ToArray();
            capturedSecond = second;
            return Exception;
        }

        try
        {
            executeAssertion(invalidValue, additionalValue, ExceptionFactory);
            throw new XunitException("The assertion should have thrown a custom exception at this point.");
        }
        catch (ExceptionDummy exception)
        {
            exception.Should().BeSameAs(exception);
            capturedFirst.Should().Equal(invalidValue.ToArray());
            capturedSecond.Should().Be(additionalValue);
        }
    }

    public static void CustomSpanException<T1, T2>(
        Span<T1> invalidValue,
        T2 additionalValue,
        ExecuteSpanAssertion<T1, T2> executeAssertion
    )
    {
        T1[]? capturedFirst = null;
        T2? capturedSecond = default;

        Exception ExceptionFactory(ReadOnlySpan<T1> parameter, T2 second)
        {
            capturedFirst = parameter.ToArray();
            capturedSecond = second;
            return Exception;
        }

        try
        {
            executeAssertion(invalidValue, additionalValue, ExceptionFactory);
            throw new XunitException("The assertion should have thrown a custom exception at this point.");
        }
        catch (ExceptionDummy exception)
        {
            exception.Should().BeSameAs(exception);
            capturedFirst.Should().Equal(invalidValue.ToArray());
            capturedSecond.Should().Be(additionalValue);
        }
    }

    public static void CustomMemoryException<T1, T2>(
        Memory<T1> invalidValue,
        T2 additionalValue,
        Action<Memory<T1>, T2, ReadOnlySpanExceptionFactory<T1, T2>> executeAssertion
    )
    {
        Memory<T1> capturedFirst = default;
        T2? capturedSecond = default;

        Exception ExceptionFactory(ReadOnlySpan<T1> first, T2 second)
        {
            capturedFirst = first.ToArray();
            capturedSecond = second;
            return Exception;
        }

        try
        {
            executeAssertion(invalidValue, additionalValue, ExceptionFactory);
            throw new XunitException("The assertion should have thrown a custom exception at this point.");
        }
        catch (ExceptionDummy exception)
        {
            exception.Should().BeSameAs(exception);
            capturedFirst.ToArray().Should().Equal(invalidValue.ToArray());
            capturedSecond.Should().Be(additionalValue);
        }
    }

    public static void CustomMemoryException<T1, T2>(
        ReadOnlyMemory<T1> invalidValue,
        T2 additionalValue,
        Action<ReadOnlyMemory<T1>, T2, ReadOnlySpanExceptionFactory<T1, T2>> executeAssertion
    )
    {
        ReadOnlyMemory<T1> capturedFirst = default;
        T2? capturedSecond = default;

        Exception ExceptionFactory(ReadOnlySpan<T1> first, T2 second)
        {
            capturedFirst = first.ToArray();
            capturedSecond = second;
            return Exception;
        }

        try
        {
            executeAssertion(invalidValue, additionalValue, ExceptionFactory);
            throw new XunitException("The assertion should have thrown a custom exception at this point.");
        }
        catch (ExceptionDummy exception)
        {
            exception.Should().BeSameAs(exception);
            capturedFirst.ToArray().Should().Equal(invalidValue.ToArray());
            capturedSecond.Should().Be(additionalValue);
        }
    }

    public static void CustomException<T1, T2, T3>(
        T1 first,
        T2 second,
        T3 third,
        Action<T1, T2, T3, Func<T1, T2, T3, Exception>> executeAssertion
    )
    {
        T1? capturedFirst = default;
        T2? capturedSecond = default;
        T3? capturedThird = default;

        Exception ExceptionFactory(T1 x, T2 y, T3 z)
        {
            capturedFirst = x;
            capturedSecond = y;
            capturedThird = z;
            return Exception;
        }

        try
        {
            executeAssertion(first, second, third, ExceptionFactory);
            throw new XunitException("The assertion should have thrown a custom exception at this point.");
        }
        catch (ExceptionDummy exception)
        {
            exception.Should().BeSameAs(Exception);
            capturedFirst.Should().Be(first);
            capturedSecond.Should().Be(second);
            capturedThird.Should().Be(third);
        }
    }

    public static void CustomMessage<TException>(Action<string> executeAssertion) where TException : Exception
    {
        const string message = "This is a custom exception message";
        try
        {
            executeAssertion(message);
            throw new XunitException("The assertion should have thrown a custom exception at this point.");
        }
        catch (TException exception)
        {
            exception.Message.Should().Contain(message);
        }
    }

    public static void WriteExceptionTo<T>(this ExceptionAssertions<T> exceptionAssertions, ITestOutputHelper output)
        where T : Exception =>
        output.WriteLine(exceptionAssertions.Which.ToString());

    private sealed class ExceptionDummy : Exception;
}
