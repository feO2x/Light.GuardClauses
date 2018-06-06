using Xunit;

namespace Light.GuardClauses.Tests
{
    public static class XunitExtensions
    {
        public static TheoryData<T> Append<T>(this TheoryData<T> theoryData, T value)
        {
            theoryData.Add(value);
            return theoryData;
        }

        public static TheoryData<T1, T2> Append<T1, T2>(this TheoryData<T1, T2> theoryData, T1 value1, T2 value2)
        {
            theoryData.Add(value1, value2);
            return theoryData;
        }
    }
}