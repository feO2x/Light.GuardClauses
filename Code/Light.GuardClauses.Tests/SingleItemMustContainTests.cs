using System;
using System.Collections.Generic;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Light.GuardClauses.Tests.CustomMessagesAndExceptions;
using Xunit;

namespace Light.GuardClauses.Tests
{
    public sealed class SingleItemMustContainTests : ICustomMessageAndExceptionTestDataProvider
    {
        [Theory(DisplayName = "MustContain must throw a CollectionException when the specified value is not part of the collection.")]
        [MemberData(nameof(CollectionDoesNotContainValueData))]
        public void CollectionDoesNotContainValue<T>(IReadOnlyCollection<T> collection, T value)
        {
            Action act = () => collection.MustContain(value, nameof(collection));

            act.ShouldThrow<CollectionException>()
               .And.Message.Should().Contain($"{nameof(collection)} must contain value \"{(value != null ? value.ToString() : "null")}\", but does not.");
        }

        public static readonly IEnumerable<object[]> CollectionDoesNotContainValueData =
            new[]
            {
                new object[] { new[] { 1, 2, 3 }, 42 },
                new object[] { new[] { "Hey", "There" }, "World" },
                new object[] { new[] { true }, false },
                new object[] { new[] { "Here", "I", "Am" }, null },
                new[] { new object[0], new object() }
            };

        [Fact(DisplayName = "MustContain must throw an ArgumentNullException when the specified collection is null.")]
        public void CollectionNull()
        {
            IReadOnlyCollection<string> collection = null;

            // ReSharper disable once ExpressionIsAlwaysNull
            Action act = () => collection.MustContain("foo");

            act.ShouldThrow<ArgumentNullException>();
        }

        [Theory(DisplayName = "MustContain must not throw an exception when the specified value is part of the collection.")]
        [MemberData(nameof(CollectionContainsValueData))]
        public void CollectionContainsValue<T>(IReadOnlyCollection<T> collection, T value)
        {
            Action act = () => collection.MustContain(value);

            act.ShouldNotThrow();
        }

        public static readonly IEnumerable<object[]> CollectionContainsValueData =
            new[]
            {
                new object[] { new[] { 1, 2, 3 }, 1 },
                new object[] { new[] { "How", "Are", "You" }, "Are" },
                new object[] { new[] { new DateTime(2016, 3, 21), new DateTime(1987, 2, 12) }, new DateTime(2016, 3, 21) }
            };

        public void PopulateTestDataForCustomExceptionAndCustomMessageTests(CustomMessageAndExceptionTestData testData)
        {
            testData.Add(new CustomExceptionTest(exception =>
                                                 {
                                                     string[] array = null;
                                                     // ReSharper disable once ExpressionIsAlwaysNull
                                                     array.MustContain("Foo", exception: exception);
                                                 }));
            testData.Add(new CustomExceptionTest(exception => new[] { 1, 2, 3 }.MustContain(42, exception: exception)));

            testData.Add(new CustomMessageTest<CollectionException>(message => new[] { 1, 2, 3 }.MustContain(42, message: message)));
            testData.Add(new CustomMessageTest<ArgumentNullException>(message =>
                                                                      {
                                                                          string[] array = null;
                                                                          // ReSharper disable once ExpressionIsAlwaysNull
                                                                          array.MustContain("Foo", message: message);
                                                                      }));
        }
    }
}