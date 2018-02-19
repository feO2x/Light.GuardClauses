using System;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Light.GuardClauses.Tests.CustomMessagesAndExceptions;
using Xunit;
using TestData = System.Collections.Generic.IEnumerable<object[]>;

namespace Light.GuardClauses.Tests.UriAssertionsTests
{
    public sealed class MustBeAbsoluteUriTests : ICustomMessageAndExceptionTestDataProvider
    {
        [Theory(DisplayName = "MustBeAbsoluteUri must not throw an exception when the specified uri is an absolute one.")]
        [MemberData(nameof(AbsoluteUriData))]
        public static void AbsoluteUri(Uri absoluteUri)
        {
            var result = absoluteUri.MustBeAbsoluteUri();

            result.Should().BeSameAs(absoluteUri);
        }

        public static readonly TestData AbsoluteUriData =
            new[]
            {
                new object[] { new Uri("http://localhost:8080/api/contacts/") },
                new object[] { new Uri("https://my.service.com/contacts/new") }
            };

        [Theory(DisplayName = "MustBeAbsoluteUri must throw an ArgumentException when the specified uri is a relative one.")]
        [MemberData(nameof(RelativeUriData))]
        public static void RelativeUri(Uri relativeUri)
        {
            Action act = () => relativeUri.MustBeAbsoluteUri(nameof(relativeUri));

            act.Should().Throw<ArgumentException>()
               .And.Message.Should().Contain($"{nameof(relativeUri)} must be an absolute URI, but it actually is \"{relativeUri}\".");
        }

        public static readonly TestData RelativeUriData =
            new[]
            {
                new object[] { new Uri("/api/orders", UriKind.Relative) },
                new object[] { new Uri("/contacts/new", UriKind.Relative) }
            };

        [Fact]
        public static void UriNull()
        {
            Action act = () => ((Uri) null).MustBeAbsoluteUri();

            act.Should().Throw<ArgumentNullException>();
        }

        [Fact(DisplayName = "MustBeAbsoluteUri must throw the custom exception with URI parameter when the specified URI is not absolute.")]
        public static void CustomExceptionWithParameter()
        {
            var relativeUri = new Uri("api/callMe", UriKind.Relative);
            var recordedUri = default(Uri);
            var exception = new Exception();

            Action act = () => relativeUri.MustBeAbsoluteUri(uri =>
            {
                recordedUri = uri;
                return exception;
            });

            act.Should().Throw<Exception>().Which.Should().BeSameAs(exception);
            recordedUri.Should().BeSameAs(relativeUri);
        }

        [Fact(DisplayName = "MustBeAbsoluteUri must not throw the custom exception with URI parameter when the specified URI is absolute.")]
        public static void NoCustomExceptionWithParameter()
        {
            var absoluteUri = new Uri("https://ravendb.net/");

            var result = absoluteUri.MustBeAbsoluteUri(uri => null);

            result.Should().BeSameAs(absoluteUri);
        }

        void ICustomMessageAndExceptionTestDataProvider.PopulateTestDataForCustomExceptionAndCustomMessageTests(CustomMessageAndExceptionTestData testData)
        {
            testData.Add(new CustomExceptionTest(exception => new Uri("/api/buildings", UriKind.Relative).MustBeAbsoluteUri(exception)))
                    .Add(new CustomMessageTest<RelativeUriException>(message => new Uri("/api/buildings", UriKind.Relative).MustBeAbsoluteUri(message: message)));
        }
    }
}