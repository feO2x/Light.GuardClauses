using System;
using FluentAssertions;
using Light.GuardClauses.Tests.CustomMessagesAndExceptions;
using Xunit;
using TestData = System.Collections.Generic.IEnumerable<object[]>;

namespace Light.GuardClauses.Tests
{
    [Trait("Category", Traits.FunctionalTests)]
    public sealed class MustBeAbsoluteUriTests : ICustomMessageAndExceptionTestDataProvider
    {
        [Theory(DisplayName = "MustBeAbsoluteUri must not throw an exception when the specified uri is an absolute one.")]
        [MemberData(nameof(AbsoluteUriData))]
        public void AbsoluteUri(Uri absoluteUri)
        {
            Action act = () => absoluteUri.MustBeAbsoluteUri();

            act.ShouldNotThrow();
        }

        public static readonly TestData AbsoluteUriData =
            new[]
            {
                new object[] { new Uri("http://localhost:8080/api/contacts/") },
                new object[] { new Uri("https://my.service.com/contacts/new") }
            };

        [Theory(DisplayName = "MustBeAbsoluteUri must throw an ArgumentException when the specified uri is a relative one.")]
        [MemberData(nameof(RelativeUriData))]
        public void RelativeUri(Uri relativeUri)
        {
            Action act = () => relativeUri.MustBeAbsoluteUri(nameof(relativeUri));

            act.ShouldThrow<ArgumentException>()
               .And.Message.Should().Contain($"{nameof(relativeUri)} must be an absolute URI, but you specified \"{relativeUri}\".");
        }

        public static readonly TestData RelativeUriData =
            new[]
            {
                new object[] { new Uri("/api/orders", UriKind.Relative) },
                new object[] { new Uri("/contacts/new", UriKind.Relative) }
            };

        [Fact]
        public void UriNull()
        {
            Action act = () => ((Uri) null).MustBeAbsoluteUri();

            act.ShouldThrow<ArgumentNullException>();
        }

        public void PopulateTestDataForCustomExceptionAndCustomMessageTests(CustomMessageAndExceptionTestData testData)
        {
            testData.Add(new CustomExceptionTest(exception => new Uri("/api/buildings", UriKind.Relative).MustBeAbsoluteUri(exception: exception)))
                    .Add(new CustomMessageTest<ArgumentException>(message => new Uri("/api/buildings", UriKind.Relative).MustBeAbsoluteUri(message: message)));
        }
    }
}