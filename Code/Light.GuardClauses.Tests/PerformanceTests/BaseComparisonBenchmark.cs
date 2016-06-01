using System.Collections.Generic;
using System.Diagnostics;
using Xunit.Abstractions;

namespace Light.GuardClauses.Tests.PerformanceTests
{
    public abstract class BaseComparisonBenchmark
    {
        protected readonly ITestOutputHelper Output;
        protected readonly Stopwatch Stopwatch = new Stopwatch();

        private ComparisonBenchmarkResultWriter _resultWriter = new ComparisonBenchmarkResultWriter();


        protected BaseComparisonBenchmark(ITestOutputHelper output)
        {
            Output = output;
        }

        public ComparisonBenchmarkResultWriter ResultWriter
        {
            get { return _resultWriter; }
            set
            {
                value.MustNotBeNull(nameof(value));
                _resultWriter = value;
            }
        }

        public void RunPerformanceTests(string testHeader, List<ComparisonCandidate> performanceCandidates, List<List<int>> testValues)
        {
            foreach (var passedValue in testValues)
            {
                foreach (var candidate in performanceCandidates)
                {
                    var duration = candidate.RunTest(passedValue);
                    candidate.Results.Add(duration);

                    Stopwatch.Reset();
                }
            }

            _resultWriter.WriteResults(testHeader, performanceCandidates, testValues, Output);
        }
    }
}