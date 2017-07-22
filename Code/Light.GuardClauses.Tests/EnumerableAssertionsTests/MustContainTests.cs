﻿using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Light.GuardClauses.FrameworkExtensions;
using Light.GuardClauses.Tests.CustomMessagesAndExceptions;
using Xunit;
using TestData = System.Collections.Generic.IEnumerable<object[]>;

namespace Light.GuardClauses.Tests.EnumerableAssertionsTests
{
    [Trait("Category", Traits.FunctionalTests)]
    public sealed class MustContainTests : ICustomMessageAndExceptionTestDataProvider
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

        [Theory(DisplayName = "MustContain must throw a CollectionException when the specified subset is not part of the collection.")]
        [MemberData(nameof(IsNoSupersetData))]
        public void IsNotSuperset<T>(T[] collection, T[] invalidSubset)
        {
            Action act = () => collection.MustContain(invalidSubset, nameof(collection));

            act.ShouldThrow<CollectionException>()
               .And.Message.Should().Contain($"{nameof(collection)} must contain the following values:{Environment.NewLine}{new StringBuilder().AppendItemsWithNewLine(invalidSubset)}");
        }

        public static readonly TestData IsNoSupersetData =
            new[]
            {
                new object[] { new object[] { 1, 2, 3, 4, 5 }, new object[] { 3, 4, 5, 6 } },
                new object[] { new object[] { 'x', 'z', 'y', 'b' }, new object[] { 'z', 'y', 'f' } }
            };

        [Theory(DisplayName = "MustContain must not throw an exception when every item in the specified subset is contained in the collection.")]
        [MemberData(nameof(IsSupersetData))]
        public void IsSuperset<T>(T[] collection, T[] subset)
        {
            Action act = () => collection.MustContain(subset);

            act.ShouldNotThrow();
        }

        public static readonly TestData IsSupersetData =
            new[]
            {
                new object[] { new object[] { 1, 2, 3, 4 }, new object[] { 2, 3 } },
                new object[] { new[] { "a", "X", "z" }, new[] { "a", "a", "a" } },
                new object[] { new[] { "a", null }, new string[] { null } },
                new object[] { new[] { "1", "2", "3" }, new[] { "3", "1", "2", "3" } }
            };

        [Theory(DisplayName = "MustContain must throw an ArgumentNullException when either the collection or the subset are null.")]
        [InlineData(new object[] { }, null)]
        [InlineData(null, new object[] { })]
        public void SupersetOrCollectionNull(object[] collection, object[] subset)
        {
            Action act = () => collection.MustContain(subset);

            act.ShouldThrow<ArgumentNullException>();
        }

        public void PopulateTestDataForCustomExceptionAndCustomMessageTests(CustomMessageAndExceptionTestData testData)
        {
            testData.Add(new CustomExceptionTest(exception => new[] { 1, 2, 3 }.MustContain(42, exception: exception)))
                    .Add(new CustomMessageTest<CollectionException>(message => new[] { 1, 2, 3 }.MustContain(42, message: message)));

            testData.Add(new CustomExceptionTest(exception => new[] { 'a', 'b' }.MustContain(new[] { 'c', 'd' }, exception: exception)))
                    .Add(new CustomMessageTest<CollectionException>(message => new[] { 'a', 'b' }.MustContain(new[] { 'c', 'd' }, message: message)));
        }
    }
}