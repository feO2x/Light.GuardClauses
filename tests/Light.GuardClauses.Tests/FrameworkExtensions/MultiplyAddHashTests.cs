using FluentAssertions;
using Light.GuardClauses.FrameworkExtensions;
using Xunit;

namespace Light.GuardClauses.Tests.FrameworkExtensions;

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
    [InlineData(null, 0, '\0', 0.0)]
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

    [Fact]
    public static void EverySupportedArityMatchesTheBuilder()
    {
        var values = new object[]
            { 1, "two", 3m, '4', 5L, 6f, 7d, (short) 8, (byte) 9, 10u, 11ul, null, 13, 14, 15, 16 };

        MultiplyAddHash.CreateHashCode(values[0], values[1], values[2], values[3], values[4])
                       .Should().Be(CreateExpectedHash(values, 5));
        MultiplyAddHash.CreateHashCode(values[0], values[1], values[2], values[3], values[4], values[5])
                       .Should().Be(CreateExpectedHash(values, 6));
        MultiplyAddHash.CreateHashCode(values[0], values[1], values[2], values[3], values[4], values[5], values[6])
                       .Should().Be(CreateExpectedHash(values, 7));
        MultiplyAddHash.CreateHashCode(
                            values[0],
                            values[1],
                            values[2],
                            values[3],
                            values[4],
                            values[5],
                            values[6],
                            values[7]
                        )
                       .Should().Be(CreateExpectedHash(values, 8));
        MultiplyAddHash.CreateHashCode(
                            values[0],
                            values[1],
                            values[2],
                            values[3],
                            values[4],
                            values[5],
                            values[6],
                            values[7],
                            values[8]
                        )
                       .Should().Be(CreateExpectedHash(values, 9));
        MultiplyAddHash.CreateHashCode(
                            values[0],
                            values[1],
                            values[2],
                            values[3],
                            values[4],
                            values[5],
                            values[6],
                            values[7],
                            values[8],
                            values[9]
                        )
                       .Should().Be(CreateExpectedHash(values, 10));
        MultiplyAddHash.CreateHashCode(
                            values[0],
                            values[1],
                            values[2],
                            values[3],
                            values[4],
                            values[5],
                            values[6],
                            values[7],
                            values[8],
                            values[9],
                            values[10]
                        )
                       .Should().Be(CreateExpectedHash(values, 11));
        MultiplyAddHash.CreateHashCode(
                            values[0],
                            values[1],
                            values[2],
                            values[3],
                            values[4],
                            values[5],
                            values[6],
                            values[7],
                            values[8],
                            values[9],
                            values[10],
                            values[11]
                        )
                       .Should().Be(CreateExpectedHash(values, 12));
        MultiplyAddHash.CreateHashCode(
                            values[0],
                            values[1],
                            values[2],
                            values[3],
                            values[4],
                            values[5],
                            values[6],
                            values[7],
                            values[8],
                            values[9],
                            values[10],
                            values[11],
                            values[12]
                        )
                       .Should().Be(CreateExpectedHash(values, 13));
        MultiplyAddHash.CreateHashCode(
                            values[0],
                            values[1],
                            values[2],
                            values[3],
                            values[4],
                            values[5],
                            values[6],
                            values[7],
                            values[8],
                            values[9],
                            values[10],
                            values[11],
                            values[12],
                            values[13]
                        )
                       .Should().Be(CreateExpectedHash(values, 14));
        MultiplyAddHash.CreateHashCode(
                            values[0],
                            values[1],
                            values[2],
                            values[3],
                            values[4],
                            values[5],
                            values[6],
                            values[7],
                            values[8],
                            values[9],
                            values[10],
                            values[11],
                            values[12],
                            values[13],
                            values[14]
                        )
                       .Should().Be(CreateExpectedHash(values, 15));
        MultiplyAddHash.CreateHashCode(
                            values[0],
                            values[1],
                            values[2],
                            values[3],
                            values[4],
                            values[5],
                            values[6],
                            values[7],
                            values[8],
                            values[9],
                            values[10],
                            values[11],
                            values[12],
                            values[13],
                            values[14],
                            values[15]
                        )
                       .Should().Be(CreateExpectedHash(values, 16));
    }

    private static int CreateExpectedHash(object[] values, int count)
    {
        var builder = MultiplyAddHashBuilder.Create();
        for (var i = 0; i < count; i++)
        {
            builder = builder.CombineIntoHash(values[i]);
        }

        return builder.BuildHash();
    }
}
