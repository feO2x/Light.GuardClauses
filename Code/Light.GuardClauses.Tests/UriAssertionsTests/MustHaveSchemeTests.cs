using System;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Light.GuardClauses.Tests.CustomMessagesAndExceptions;
using Xunit;
using TestData = System.Collections.Generic.IEnumerable<object[]>;

namespace Light.GuardClauses.Tests.UriAssertionsTests
{
    public sealed class MustHaveSchemeTests : ICustomMessageAndExceptionTestDataProvider
    {
        [Theory(DisplayName = "MustHaveScheme must throw an InvalidUriSchemeException when the URI does not have the specified scheme.")]
        [MemberData(nameof(InvalidSchemeData))]
        public static void InvalidScheme(Uri uri, string scheme)
        {
            Action act = () => uri.MustHaveScheme(scheme, nameof(uri));

            act.Should().Throw<InvalidUriSchemeException>()
               .And.Message.Should().Contain($"{nameof(uri)} must use the scheme \"{scheme}\"");
        }

        public static readonly TestData InvalidSchemeData =
            new[]
            {
                new object[] { new Uri("http://localhost:8080"), "https" },
                new object[] { new Uri("http://my.service.com/upload"), "ftp" },
            };

        [Theory(DisplayName = "MustHaveScheme must not throw an exception when the specified scheme is used by the URI.")]
        [MemberData(nameof(ValidSchemeData))]
        public static void ValidScheme(Uri uri, string scheme)
        {
            var result = uri.MustHaveScheme(scheme);

            result.Should().BeSameAs(uri);
        }

        public static readonly TestData ValidSchemeData =
            new[]
            {
                new object[] { new Uri("https://www.google.com"), "https" },
                new object[] { new Uri("ftps://192.168.177.2"), "ftps" }
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

        [Fact(DisplayName = "MustHaveScheme must throw the custom exception with URI parameter when the URI does not use the specified scheme.")]
        public static void CustomExceptionWithUri()
        {
            var url = new Uri("https://www.microsoft.com");
            var recordedUri = default(Uri);
            var exception = new Exception();

            Action act = () => url.MustHaveScheme("ftp", uri =>
            {
                recordedUri = uri;
                return exception;
            });

            act.Should().Throw<Exception>().Which.Should().BeSameAs(exception);
            recordedUri.Should().BeSameAs(url);
        }

        [Fact(DisplayName = "MustHaveScheme must not throw the custom exception with URI parameter when the URI's scheme is equal to the specified scheme.")]
        public static void NoExceptionWithUriThrown()
        {
            var ftpUri = new Uri("ftp://myserver.group");

            var result = ftpUri.MustHaveScheme("ftp", uri => null);

            result.Should().BeSameAs(ftpUri);
        }

        [Fact(DisplayName = "MustHaveScheme must throw the custom exception with URI and scheme when the URI does not use the specified scheme.")]
        public static void CustomExceptionWithUriAndScheme()
        {
            var url = new Uri("https://www.microsoft.com");
            var urlScheme = "http";
            var recordedUri = default(Uri);
            var recodedScheme = default(string);
            var exception = new Exception();

            Action act = () => url.MustHaveScheme(urlScheme, (uri, scheme) =>
            {
                recordedUri = uri;
                recodedScheme = scheme;
                return exception;
            });

            act.Should().Throw<Exception>().Which.Should().BeSameAs(exception);
            recordedUri.Should().BeSameAs(url);
            recodedScheme.Should().BeSameAs(urlScheme);
        }

        [Fact(DisplayName = "MustHaveScheme must not throw the custom exception with URI and scheme when the URI's scheme is equal to the specified scheme.")]
        public static void NoExceptionWithUriAndScheme()
        {
            var url = new Uri("http://www.sgb-smit.group");

            var result = url.MustHaveScheme("http", (uri, scheme) => null);

            result.Should().BeSameAs(url);
        }

        void ICustomMessageAndExceptionTestDataProvider.PopulateTestDataForCustomExceptionAndCustomMessageTests(CustomMessageAndExceptionTestData testData)
        {
            testData.Add(new CustomExceptionTest(exception => new Uri("http://localhost").MustHaveScheme("https", exception)))
                    .Add(new CustomMessageTest<InvalidUriSchemeException>(message => new Uri("http://localhost").MustHaveScheme("https", message: message)));
        }
    }
}