using System;
using System.Collections.Generic;
using System.Linq;

namespace Light.GuardClauses.Tests.PerformanceTests
{
    public sealed class CounterPerformanceCandidate
    {
        public readonly string Name;
        public readonly List<CounterTestRunResult> TestRunResults = new List<CounterTestRunResult>();
        public readonly Func<CounterTestRunResult> RunTest;

        public CounterPerformanceCandidate(string name, Func<CounterTestRunResult> runTest)
        {
            name.MustNotBeNullOrWhiteSpace(nameof(name));
            runTest.MustNotBeNull(nameof(runTest));

            Name = name;
            RunTest = runTest;
        }

        public double GetAverageExecutionsPerMillisecond()
        {
            return TestRunResults.Select(r => r.NumberOfCalls / r.ElapsedTime.TotalMilliseconds)
                                 .Average();
        }
    }
}