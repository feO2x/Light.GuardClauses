using System;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Xunit;

namespace Light.GuardClauses.Tests.UriAssertions
{
    public static class MustHaveSchemeTests
    {
        [Theory(DisplayName = "MustHaveScheme must throw an InvalidUriSchemeException when the URI does not have the specified scheme.")]
        [MemberData(nameof(InvalidSchemeData))]
        public static void InvalidScheme(Uri uri, string scheme)
        {
            Action act = () => uri.MustHaveScheme(scheme, nameof(uri));

            act.Should().Throw<InvalidUriSchemeException>()
               .And.Message.Should().Contain($"{nameof(uri)} must use the scheme \"{scheme}\"");
        }

        public static readonly TheoryData<Uri, string> InvalidSchemeData =
            new TheoryData<Uri, string>
            {
                { new Uri("http://localhost:8080"), "https" },
                { new Uri("http://my.service.com/upload"), "ftp" }
            };

        [Theory(DisplayName = "MustHaveScheme must not throw an exception when the specified scheme is used by the URI.")]
        [MemberData(nameof(ValidSchemeData))]
        public static void ValidScheme(Uri uri, string scheme)
        {
            var result = uri.MustHaveScheme(scheme);

            result.Should().BeSameAs(uri);
        }

        public static readonly TheoryData<Uri, string> ValidSchemeData =
            new TheoryData<Uri, string>
            {
                { new Uri("https://www.google.com"), "https" },
                { new Uri("ftps://192.168.177.2"), "ftps" }
            };

        [Fact(DisplayName = "MustHaveScheme must throw an ArgumentNullException when the specified URI is null.")]
        public static void UriNull()
        {
            Action act = () => ((Uri) null).MustHaveScheme("foo");

            act.Should().Throw<ArgumentNullException>();
        }

        [Fact(DisplayName = "MustHaveScheme must throw an RelativeUriException when the specified URI is relative.")]
        public static void UriRelative()
        {
            Action act = () => new Uri("/api/foo", UriKind.Relative).MustHaveScheme("https");

            act.Should().Throw<RelativeUriException>();
        }

        [Fact]
        public static void CustomException() =>
            Test.CustomException(new Uri("https://www.microsoft.com"),
                                 "http",
                                 (url, scheme, exceptionFactory) => url.MustHaveScheme(scheme, exceptionFactory));

        [Fact]
        public static void CustomMessage() =>
            Test.CustomMessage<InvalidUriSchemeException>(message => new Uri("ftp://foo.com").MustHaveScheme("https", message: message));
    }
}