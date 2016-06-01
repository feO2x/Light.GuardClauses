using System;
using System.Collections.Generic;

namespace Light.GuardClauses.Tests.PerformanceTests
{
    public class ComparisonCandidate
    {
        public readonly string Name;
        public readonly List<TimeSpan> Results = new List<TimeSpan>();
        public readonly Func<List<int>, TimeSpan> RunTest;

        public ComparisonCandidate(string name, Func<List<int>, TimeSpan> runTest)
        {
            Name = name;
            RunTest = runTest;
        }
    }
}