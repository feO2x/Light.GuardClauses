using System;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Xunit;

namespace Light.GuardClauses.Tests.UriAssertions
{
    public static class MustBeAbsoluteUriTests
    {
        [Theory(DisplayName = "MustBeAbsoluteUri must not throw an exception when the specified uri is an absolute one.")]
        [MemberData(nameof(AbsoluteUriData))]
        public static void AbsoluteUri(Uri absoluteUri)
        {
            var result = absoluteUri.MustBeAbsoluteUri();

            result.Should().BeSameAs(absoluteUri);
        }

        public static readonly TheoryData<Uri> AbsoluteUriData =
            new TheoryData<Uri>
            {
                new Uri("http://localhost:8080/api/contacts/"),
                new Uri("https://my.service.com/contacts/new"),
                new Uri("ftp://172.20.10.5/")
            };

        [Theory(DisplayName = "MustBeAbsoluteUri must throw an ArgumentException when the specified uri is a relative one.")]
        [MemberData(nameof(RelativeUriData))]
        public static void RelativeUri(Uri relativeUri)
        {
            Action act = () => relativeUri.MustBeAbsoluteUri(nameof(relativeUri));

            act.Should().Throw<ArgumentException>()
               .And.Message.Should().Contain($"{nameof(relativeUri)} must be an absolute URI, but it actually is \"{relativeUri}\".");
        }

        public static readonly TheoryData<Uri> RelativeUriData =
            new TheoryData<Uri>
            {
                new Uri("/api/orders", UriKind.Relative),
                new Uri("/contacts/new", UriKind.Relative)
            };

        [Fact]
        public static void UriNull()
        {
            Action act = () => ((Uri) null).MustBeAbsoluteUri();

            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public static void CustomException() =>
            Test.CustomException(new Uri("/api/foo", UriKind.Relative),
                                 (uri, exceptionFactory) => uri.MustBeAbsoluteUri(exceptionFactory));

        [Fact]
        public static void CustomExceptionUriAbsolute()
        {
            var uri = new Uri("https://www.microsoft.com");
            uri.MustBeAbsoluteUri(_ => null).Should().BeSameAs(uri);
        }

        [Fact]
        public static void CustomMessage() =>
            Test.CustomMessage<RelativeUriException>(message => new Uri("/home", UriKind.Relative).MustBeAbsoluteUri(message: message));

        [Fact]
        public static void CustomMessageUriNull() => 
            Test.CustomMessage<ArgumentNullException>(message => ((Uri)null).MustBeAbsoluteUri(message: message));
    }
}