using System;
using FluentAssertions;
using Xunit;

namespace Light.GuardClauses.Tests.CommonAssertions;

public static class UuidVersion7Tests
{
    private static readonly Guid RfcExample = Guid.Parse("017f22e2-79b0-7cc3-98c4-dc0c0c07398f");
    private static readonly Guid WrongVariant = Guid.Parse("017f22e2-79b0-7cc3-18c4-dc0c0c07398f");

    [Fact]
    public static void RfcExampleIsUuidVersion7() => RfcExample.IsUuidVersion7().Should().BeTrue();

#if NET9_0_OR_GREATER
    [Fact]
    public static void FrameworkGeneratedValueIsUuidVersion7() => Guid.CreateVersion7().IsUuidVersion7().Should().BeTrue();
#endif

    [Theory]
    [MemberData(nameof(InvalidValues))]
    public static void InvalidStructuresAreRejected(Guid value) => value.IsUuidVersion7().Should().BeFalse();

    public static TheoryData<Guid> InvalidValues => [Guid.Empty, Guid.NewGuid(), WrongVariant];

    [Fact]
    public static void GuardReturnsOriginalValue() => RfcExample.MustBeUuidVersion7().Should().Be(RfcExample);

    [Fact]
    public static void DefaultExceptionCapturesExpression()
    {
        var invalidUuid = WrongVariant;

        var act = () => invalidUuid.MustBeUuidVersion7();

        act.Should().Throw<ArgumentException>()
           .WithParameterName(nameof(invalidUuid))
           .WithMessage("*RFC/IETF UUID version 7*");
    }

    [Fact]
    public static void CustomMessage() =>
        Test.CustomMessage<ArgumentException>(message => Guid.Empty.MustBeUuidVersion7(message: message));

    [Fact]
    public static void CustomFactoryReceivesValue() =>
        Test.CustomException(WrongVariant, (value, factory) => value.MustBeUuidVersion7(factory));
}
