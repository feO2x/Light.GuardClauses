using System;
using System.IO;
using System.Text;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Xunit;

namespace Light.GuardClauses.Tests.CommonAssertions;

public static class MustBeOfTypeTests
{
    [Fact]
    public static void CastNotPossible()
    {
        object reference = "Foo";

        Action act = () => reference.MustBeOfType<Array>(nameof(reference));

        var exceptionAssertions = act.Should().Throw<TypeCastException>().Which;
        exceptionAssertions.Message.Should().Contain($"{nameof(reference)} \"{reference}\" cannot be cast to \"{typeof(Array)}\".");
        exceptionAssertions.ParamName.Should().BeSameAs(nameof(reference));
    }

    [Fact]
    public static void Downcast() =>
        "Bar".MustBeOfType<string>().Should().BeSameAs("Bar");

    [Fact]
    public static void Cast() =>
        "Baz".MustBeOfType<IConvertible>().Should().BeSameAs("Baz");

    [Fact]
    public static void ReferenceIsNull()
    {
        Action act = () => ((object) null).MustBeOfType<string>("Foo");

        act.Should().Throw<ArgumentNullException>()
           .WithParameterName("Foo");
    }

    [Fact]
    public static void CustomException() =>
        Test.CustomException<object>("Foo",
                                     (value, exceptionFactory) => value.MustBeOfType<Encoding>(exceptionFactory));


    [Fact]
    public static void CustomExceptionArgumentNull() =>
        Test.CustomException<object>(null,
                                     (nullReference, exceptionFactory) => nullReference.MustBeOfType<string>(exceptionFactory));

    [Fact]
    public static void CustomExceptionDowncastValid()
    {
        var encoding = Encoding.UTF8;
        encoding.MustBeOfType<UTF8Encoding>(_ => null).Should().BeSameAs(encoding);
    }

    [Fact]
    public static void CustomMessage() =>
        Test.CustomMessage<TypeCastException>(message => "Foo".MustBeOfType<StreamReader>(message: message));

    [Fact]
    public static void CustomMessageArgumentNull() =>
        Test.CustomMessage<ArgumentNullException>(message => ((string) null).MustBeOfType<Array>(message: message));

    [Fact]
    public static void CallerArgumentExpressionForTypeCastException()
    {
        var someValue = (object) "Foo";

        Action act = () => someValue.MustBeOfType<Exception>();

        act.Should().Throw<TypeCastException>()
           .WithParameterName(nameof(someValue));
    }

    [Fact]
    public static void CallerArgumentExpressionForArgumentNullException()
    {
        var myValue = (object) null;

        // ReSharper disable once ExpressionIsAlwaysNull
        Action act = () => myValue.MustBeOfType<string>();

        act.Should().Throw<ArgumentNullException>()
           .WithParameterName(nameof(myValue));
    }
}