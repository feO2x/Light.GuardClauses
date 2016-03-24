using System;
using FluentAssertions;
using Light.GuardClauses.Tests.CustomMessagesAndExceptions;
using Xunit;
using TestData = System.Collections.Generic.IEnumerable<object[]>;

namespace Light.GuardClauses.Tests
{
    public sealed class MustNotBeNullTests : ICustomMessageAndExceptionTestDataProvider
    {
        [Fact(DisplayName = "MustNotBeNull must throw an ArgumentNullException when null is provided.")]
        public void NullIsGiven()
        {
            Action act = () => DummyMethod<string>(null);

            act.ShouldThrow<ArgumentNullException>()
               .And.ParamName.Should().Be("someObject");
        }

        [Fact(DisplayName = "MustNotBeNull must provide a default message for the ArgumentNullException.")]
        public void NullMessage()
        {
            object @object = null;

            // ReSharper disable once ExpressionIsAlwaysNull
            Action act = () => @object.MustNotBeNull(nameof(@object));

            act.ShouldThrow<ArgumentNullException>()
               .And.Message.Should().Contain($"{nameof(@object)} must not be null.");
        }

        [Theory(DisplayName = "MustNotBeNull must not throw an exception when the specified reference is not null.")]
        [MemberData(nameof(ObjectReferenceTestData))]
        public void ValidObjectReferenceIsGiven<T>(T value) where T : class
        {
            Action act = () => DummyMethod(value);

            act.ShouldNotThrow();
        }

        public static readonly TestData ObjectReferenceTestData =
            new[]
            {
                new object[] { string.Empty },
                new[] { new object() }
            };

        private static void DummyMethod<T>(T someObject) where T : class
        {
            someObject.MustNotBeNull(nameof(someObject));
        }

        public void PopulateTestDataForCustomExceptionAndCustomMessageTests(CustomMessageAndExceptionTestData testData)
        {
            testData.Add(new CustomExceptionTest(exception => ((object) null).MustNotBeNull(exception: exception)));

            testData.Add(new CustomMessageTest<ArgumentNullException>(message => ((object) null).MustNotBeNull(message: message)));
        }
    }
}