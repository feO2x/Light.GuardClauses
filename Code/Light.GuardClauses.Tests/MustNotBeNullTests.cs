using System;
using FluentAssertions;
using Xunit;
using TestData = System.Collections.Generic.IEnumerable<object[]>;

namespace Light.GuardClauses.Tests
{
    public sealed class MustNotBeNullTests
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

        [Fact(DisplayName = "The caller can specify a custom message that MustNotBeNull must inject instead of the default one.")]
        public void CustomMessage()
        {
            object someObject = null;
            const string message = "Thou shall not be null!";

            // ReSharper disable once ExpressionIsAlwaysNull
            Action act = () => someObject.MustNotBeNull(message: message);

            act.ShouldThrow<ArgumentNullException>()
               .And.Message.Should().Be(message);
        }

        [Fact(DisplayName = "The caller can specify a custom exception that MustNotBeNull must raise instead of the default one.")]
        public void CustomException()
        {
            object someObject = null;
            var exception = new Exception();

            // ReSharper disable once ExpressionIsAlwaysNull
            Action act = () => someObject.MustNotBeNull(exception: exception);

            act.ShouldThrow<Exception>().Which.Should().BeSameAs(exception);
        }

        private static void DummyMethod<T>(T someObject) where T : class
        {
            someObject.MustNotBeNull(nameof(someObject));
        }
    }
}