using System;
using FluentAssertions;
using Xunit;
using TestData = System.Collections.Generic.IEnumerable<object[]>;

namespace Light.GuardClauses.Tests
{
    public sealed class NotNullTests
    {
        [Fact(DisplayName = "The constructor of NotNull must throw an ArgumentNullException when null is specified.")]
        public void ObjectNull()
        {
            // ReSharper disable once ObjectCreationAsStatement
            Action act = () => new NotNull<object>(null);

            act.ShouldThrow<ArgumentNullException>().
                And.ParamName.Should().Be("object");
        }

        [Theory(DisplayName = "The referenced passed to the constructor must be retrievable if it is not null.")]
        [MemberData(nameof(ValidReferenceData))]
        public void ValidReferenceRetrievable<T>(T @object) where T : class
        {
            var notNull = new NotNull<T>(@object);

            notNull.Object.Should().BeSameAs(@object);
        }

        [Theory(DisplayName = "The hash code produced by a NotNull instance must be equal to the hash code of the referenced object.")]
        [MemberData(nameof(ValidReferenceData))]
        public void GetHashCodeCallForwared<T>(T @object) where T : class
        {
            var notNull = new NotNull<T>(@object);

            notNull.GetHashCode().Should().Be(@object.GetHashCode());
        }

        [Theory(DisplayName = "Equals must return true when the specified object is the same instance as the referenced one.")]
        [MemberData(nameof(ValidReferenceData))]
        public void EqualsSameReference<T>(T @object) where T : class
        {
            var notNull = new NotNull<T>(@object);

            notNull.Equals(@object).Should().BeTrue();
        }

        [Theory(DisplayName = "Equals must return true when the specified object is equal to the referenced one, else false.")]
        [InlineData("hey", "hey", true)]
        [InlineData("1", "1", true)]
        [InlineData("hey", "ho", false)]
        [InlineData("Hello", "World", false)]
        [InlineData("Hello", null, false)]
        public void EqualsWithDifferentReferences(string first, string second, bool expected)
        {
            var notNull = new NotNull<string>(first);

            notNull.Equals(second).Should().Be(expected);
        }

        [Theory(DisplayName = "Equals must return true when the corresponding referenced objects of two distinct NotNull instances are considered equal, else false.")]
        [InlineData("hey", "hey", true)]
        [InlineData("1", "1", true)]
        [InlineData("hey", "ho", false)]
        [InlineData("Hello", "World", false)]
        public void EqualsWithDifferentInstances(string first, string second, bool expected)
        {
            var firstNotNull = new NotNull<string>(first);
            var secondNotNull = new NotNull<string>(second);

            firstNotNull.Equals(secondNotNull).Should().Be(expected);
        }

        [Theory(DisplayName = "Object.Equals must return true if it is another instance of NotNull with the same referenced object, or the referenced object; otherwise false must be returned.")]
        [MemberData(nameof(EqualsWithObjectData))]
        public void EqualsWithObject(string @string, object other, bool expected)
        {
            NotNull<string> notNull = @string;

            notNull.Equals(other).Should().Be(expected);
        }

        public static readonly TestData EqualsWithObjectData =
            new[]
            {
                new object[] { "Hello", "Hello", true },
                new object[] { "Test", new NotNull<string>("Test"), true },
                new object[] { "1", "2", false },
                new object[] { "1", new NotNull<string>("42"), false },
                new object[] { "Check this", new Random(), false },
                new object[] { "Check more", null, false }
            };

        [Fact(DisplayName = "Implicit conversions from reference to NotNull instance and vice versa must be possible.")]
        public void ImplicitConversion()
        {
            const string @string = "Even Joffrey? Even Joffrey.";
            NotNull<string> notNull = @string;

            notNull.Object.Should().BeSameAs(@string);

            string objectFromImplicitConversion = notNull;

            objectFromImplicitConversion.Should().BeSameAs(@string);
        }

        [Fact(DisplayName = "Implicit conversion from NotNull instance to interface reference is not possible and must be circumvented by using NotNull.Object.")]
        public void ImplicitConversionWithInterfaces()
        {
            const string @string = "Money buys a man's silence for a time. A bolt in the heart buys it forever.";
            NotNull<IComparable> notNull = @string;
            notNull.Object.Should().BeSameAs(@string);

            IComparable comparable = @string;
            // The following implicit conversion from interface reference to NotNullInstance is not allowed because the C# specification forbids to implicitely convert to an interface type: http://stackoverflow.com/questions/143485/implicit-operator-using-interfaces
            // NotNull<IComparable> notNull2 = comparable;
            var notNull2 = comparable.AsNotNull();
            notNull2.Object.Should().BeSameAs(@string);

            // This conversion is not allowed because of the same reason
            // IComparable objectFromImplicitConversion = notNull;
            var convertedObject = notNull.Object;
            convertedObject.MustBeSameAs(@string);
        }

        [Fact(DisplayName = "The equality operators must be overloaded so that NotNull instances can be compared with each other, as well as single NotNull instances with references of the corresponding type.")]
        public void EqualityOperators()
        {
            const string @string = "Which is the bigger number, five or one?";
            const string otherString = "Our purpose died with the Mad King.";
            string nullString = null;
            NotNull<string> notNull = @string;
            NotNull<string> otherNotNull = otherString;

            // ReSharper disable ExpressionIsAlwaysNull
            (notNull == @string).Should().BeTrue();
            (@string == notNull).Should().BeTrue();
            (notNull == otherString).Should().BeFalse();
            (otherString == notNull).Should().BeFalse();
            (notNull == otherNotNull).Should().BeFalse();
            (notNull == nullString).Should().BeFalse();
            (nullString == notNull).Should().BeFalse();

            (notNull != @string).Should().BeFalse();
            (@string != notNull).Should().BeFalse();
            (notNull != otherString).Should().BeTrue();
            (otherString != notNull).Should().BeTrue();
            (notNull != otherNotNull).Should().BeTrue();
            (notNull != nullString).Should().BeTrue();
            (nullString != notNull).Should().BeTrue();
            // ReSharper restore ExpressionIsAlwaysNull
        }

        [Theory(DisplayName = "The string representation of a NotNull instance must be the same as the one of the referenced object.")]
        [MemberData(nameof(ValidReferenceData))]
        public void ToStringCallForwarded<T>(T @object) where T : class
        {
            NotNull<T> notNull = @object;

            notNull.ToString().Should().Be(@object.ToString());
        }

        public static readonly TestData ValidReferenceData =
            new[]
            {
                new object[] { "Hello" },
                new[] { new object() },
                new object[] { new Random() }
            };
    }
}