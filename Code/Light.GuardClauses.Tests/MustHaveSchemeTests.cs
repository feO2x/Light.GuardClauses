using System;
using FluentAssertions;
using Light.GuardClauses.Tests.CustomMessagesAndExceptions;
using Xunit;
using TestData = System.Collections.Generic.IEnumerable<object[]>;

namespace Light.GuardClauses.Tests
{
    [Trait("Category", Traits.FunctionalTests)]
    public sealed class MustHaveSchemeTests : ICustomMessageAndExceptionTestDataProvider
    {
        [Theory(DisplayName = "MustHaveScheme must throw an ArgumentException when the URI does not have the specified scheme.")]
        [MemberData(nameof(InvalidSchemeData))]
        public void InvalidScheme(Uri uri, string scheme)
        {
            Action act = () => uri.MustHaveScheme(scheme, nameof(uri));

            act.ShouldThrow<ArgumentException>()
               .And.Message.Should().Contain($"{nameof(uri)} must have scheme \"{scheme}\"");
        }

        public static readonly TestData InvalidSchemeData =
            new[]
            {
                new object[] { new Uri("http://localhost:8080"), "https" },
                new object[] { new Uri("http://my.service.com/upload"), "ftp" },
                new object[] { new Uri("/api/contacts", UriKind.Relative), "https" }
            };

        [Theory]
        [MemberData(nameof(ValidSchemeData))]
        public void ValidScheme(Uri uri, string scheme)
        {
            Action act = () => uri.MustHaveScheme(scheme);

            act.ShouldNotThrow();
        }

        public static readonly TestData ValidSchemeData =
            new[]
            {
                new object[] { new Uri("https://www.google.com"), "https" },
                new object[] { new Uri("ftps://192.168.177.2"), "ftps" }
            };

        [Fact]
        public void UriNull()
        {
            Action act = () => ((Uri) null).MustHaveScheme("foo");

            act.ShouldThrow<ArgumentNullException>();
        }

        public void PopulateTestDataForCustomExceptionAndCustomMessageTests(CustomMessageAndExceptionTestData testData)
        {
            testData.Add(new CustomExceptionTest(exception => new Uri("http://localhost").MustHaveScheme("https", exception: exception)))
                    .Add(new CustomMessageTest<ArgumentException>(message => new Uri("http://localhost").MustHaveScheme("https", message: message)));
        }
    }
}