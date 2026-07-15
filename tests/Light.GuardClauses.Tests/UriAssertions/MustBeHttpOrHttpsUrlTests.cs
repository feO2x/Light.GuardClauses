using System;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Xunit;

namespace Light.GuardClauses.Tests.UriAssertions;

public static class MustBeHttpOrHttpsUrlTests
{
    [Theory]
    [InlineData("http://example.com/")]
    [InlineData("https://example.com/")]
    public static void HttpAndHttpsUrlsAreAccepted(string url)
    {
        var uri = new Uri(url);

        uri.MustBeHttpOrHttpsUrl().Should().BeSameAs(uri);
    }

    [Fact]
    public static void OtherSchemeIsRejected()
    {
        var ftpUrl = new Uri("ftp://example.com");

        var act = () => ftpUrl.MustBeHttpOrHttpsUrl();

        act.Should().Throw<InvalidUriSchemeException>()
           .WithParameterName(nameof(ftpUrl));
    }

    [Fact]
    public static void CustomMessage() =>
        Test.CustomMessage<InvalidUriSchemeException>(
            message => new Uri("ftp://example.com").MustBeHttpOrHttpsUrl(message: message)
        );

    [Fact]
    public static void CustomException() =>
        Test.CustomException(
            new Uri("ftp://example.com"),
            (url, exceptionFactory) => url.MustBeHttpOrHttpsUrl(exceptionFactory)
        );

    [Fact]
    public static void CustomExceptionNotThrown()
    {
        var url = new Uri("https://example.com");

        url.MustBeHttpOrHttpsUrl(_ => new ()).Should().BeSameAs(url);
    }
}
