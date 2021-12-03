using FluentAssertions;
using Light.GuardClauses.FrameworkExtensions;
using Xunit;

namespace Light.GuardClauses.Tests.FrameworkExtensions
{
    public static class MultiplyAddHashTests
    {
        [Theory]
        [InlineData("Foo", 42)]
        [InlineData("Bar", -177422)]
        [InlineData(null, 0)]
        public static void TwoParameters(string value1, int value2)
        {
            var hashCode1 = MultiplyAddHash.CreateHashCode(value1, value2);
            var hashCode2 = MultiplyAddHashBuilder.Create()
                                                  .CombineIntoHash(value1)
                                                  .CombineIntoHash(value2)
                                                  .BuildHash();

            hashCode1.Should().Be(hashCode2);
        }

        [Theory]
        [InlineData("Foo", 42, 'a')]
        [InlineData("Bar", -177422, 'Y')]
        [InlineData(null, 0, default(char))]
        public static void ThreeParameters(string value1, int value2, char value3)
        {
            var hashCode1 = MultiplyAddHash.CreateHashCode(value1, value2, value3);
            var hashCode2 = MultiplyAddHashBuilder.Create()
                                                  .CombineIntoHash(value1)
                                                  .CombineIntoHash(value2)
                                                  .CombineIntoHash(value3)
                                                  .BuildHash();

            hashCode1.Should().Be(hashCode2);
        }

        [Theory]
        [InlineData("Foo", 42, 'a', 87.73665)]
        [InlineData("Bar", -177422, 'Y', -15.25)]
        [InlineData(null, 0, default(char), 0.0)]
        public static void FourParameters(string value1, int value2, char value3, double value4)
        {
            var hashCode1 = MultiplyAddHash.CreateHashCode(value1, value2, value3, value4);
            var hashCode2 = MultiplyAddHashBuilder.Create()
                                                  .CombineIntoHash(value1)
                                                  .CombineIntoHash(value2)
                                                  .CombineIntoHash(value3)
                                                  .CombineIntoHash(value4)
                                                  .BuildHash();

            hashCode1.Should().Be(hashCode2);
        }
    }
}