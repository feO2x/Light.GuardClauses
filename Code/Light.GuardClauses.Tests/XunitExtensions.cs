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
    }
}