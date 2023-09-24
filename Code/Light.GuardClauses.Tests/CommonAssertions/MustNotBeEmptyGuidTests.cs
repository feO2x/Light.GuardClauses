using System;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Xunit;

namespace Light.GuardClauses.Tests.CommonAssertions;

public static class MustNotBeEmptyGuidTests
{
    [Fact]
    public static void GuidEmpty()
    {
        var emptyGuid = Guid.Empty;

        Action act = () => emptyGuid.MustNotBeEmpty(nameof(emptyGuid));

        act.Should().Throw<EmptyGuidException>()
           .And.ParamName.Should().Be(nameof(emptyGuid));
    }

    [Fact]
    public static void GuidNotEmpty()
    {
        var validGuid = Guid.NewGuid();

        var result = validGuid.MustNotBeEmpty();

        result.Should().Be(validGuid);
    }

    [Fact]
    public static void CustomException() =>
        Test.CustomException(exceptionFactory => Guid.Empty.MustNotBeEmpty(exceptionFactory));

    [Fact]
    public static void CustomMessage() =>
        Test.CustomMessage<EmptyGuidException>(message => Guid.Empty.MustNotBeEmpty(message: message));

    [Fact]
    public static void CallerArgumentExpression()
    {
        var emptyGuid = Guid.Empty;

        Action act = () => emptyGuid.MustNotBeEmpty();

        act.Should().Throw<EmptyGuidException>()
           .And.ParamName.Should().Be(nameof(emptyGuid));
    }

    public class Entity(Guid id)
    {
        public Guid Id { get; } = id.MustNotBeEmpty();
    }
    
    [Fact]
    public static void PrimaryConstructorValidArgument()
    {
        var guid = Guid.NewGuid();
        var entity = new Entity(guid);

        entity.Id.Should().Be(guid);
    }

    [Fact]
    public static void PrimaryConstructorInvalidArgument()
    {
        var act = () => new Entity(Guid.Empty);

        act.Should().Throw<EmptyGuidException>()
           .And.Message.Should().StartWith("id must be a valid GUID, but it actually is an empty one.");
    }
}

