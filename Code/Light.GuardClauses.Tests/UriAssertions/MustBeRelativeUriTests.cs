using System;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Xunit;

namespace Light.GuardClauses.Tests.UriAssertions;

public static class MustBeRelativeUriTests
{
    [Theory]
    [MemberData(nameof(AbsoluteUris))]
    public static void UriAbsolute(Uri absoluteUri)
    {
        Action act = () => absoluteUri.MustBeRelativeUri(nameof(absoluteUri));

        act.Should().Throw<AbsoluteUriException>()
           .And.Message.Should().Contain($"{nameof(absoluteUri)} must be a relative URI, but it actually is \"{absoluteUri}\".");
    }

    public static readonly TheoryData<Uri> AbsoluteUris =
        new()
        {
            new Uri("https://www.microsoft.com"),
            new Uri("https://xunit.github.io/")
        };

    [Theory]
    [MemberData(nameof(RelativeUris))]
    public static void UriRelative(Uri relativeUri) =>
        relativeUri.MustBeRelativeUri().Should().BeSameAs(relativeUri);

    public static readonly TheoryData<Uri> RelativeUris =
        new()
        {
            new Uri("api/login", UriKind.Relative),
            new Uri("/home", UriKind.Relative)
        };

    [Fact]
    public static void UriNull()
    {
        Action act = () => ((Uri) null).MustBeRelativeUri();

        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public static void CustomException() =>
        Test.CustomException(new Uri("fttp://server.com"),
                             (uri, exceptionFactory) => uri.MustBeRelativeUri(exceptionFactory));

    [Fact]
    public static void CustomExceptionUriRelative()
    {
        var url = new Uri("api/tweets", UriKind.Relative);
        url.MustBeRelativeUri(_ => null).Should().BeSameAs(url);
    }

    [Fact]
    public static void CustomMessage() =>
        Test.CustomMessage<AbsoluteUriException>(message => new Uri("ssh://foo.com").MustBeRelativeUri(message: message));

    [Fact]
    public static void CustomMessageUriNull() =>
        Test.CustomMessage<ArgumentNullException>(message => ((Uri) null).MustBeRelativeUri(message: message));

    [Fact]
    public static void CallerArgumentExpression()
    {
        var absoluteUri = new Uri("https://www.duckduckgo.com", UriKind.Absolute);

        var act = () => absoluteUri.MustBeRelativeUri();

        act.Should().Throw<AbsoluteUriException>()
           .And.ParamName.Should().Be(nameof(absoluteUri));
    }
}