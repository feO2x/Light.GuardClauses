using System;
using System.Collections.Generic;
using System.Linq;
using Xunit.Abstractions;

namespace Light.GuardClauses.Tests.PerformanceTests
{
    public class CounterComparisonResultWriter
    {
        public virtual void WriteResults(string testHeader,
                                         IList<CounterPerformanceCandidate> orderedCandidates,
                                         IList<TimeSpan> performanceTestLengths,
                                         ITestOutputHelper output)
        {
            output.WriteLine(testHeader);
            output.WriteLine(new string('-', 40));
            output.WriteLine(string.Empty);

            for (var i = 0; i < performanceTestLengths.Count; i++)
            {
                var length = performanceTestLengths[i];
                output.WriteLine($"{length.TotalMilliseconds}ms test run:");

                foreach (var performanceCandidate in orderedCandidates)
                {
                    var testRunResult = performanceCandidate.TestRunResults[i];
                    output.WriteLine($"{performanceCandidate.Name}: {testRunResult.NumberOfCalls:N0} executions (in {testRunResult.ElapsedTime.TotalMilliseconds:N0}ms)");
                }
                output.WriteLine(string.Empty);
                output.WriteLine(string.Empty);
            }
        }

        public virtual void WriteAverageBenchmarkStatistics(IList<CounterPerformanceCandidate> orderedCandidates, ITestOutputHelper output)
        {
            var best = orderedCandidates.First();
            output.WriteLine($"Average for {best.Name}: {best.GetAverageExecutionsPerMillisecond():N2} executions per ms");

            for (var i = 1; i < orderedCandidates.Count; i++)
            {
                var candidate = orderedCandidates[i];
                var ratioToBest = candidate.GetAverageExecutionsPerMillisecond() / best.GetAverageExecutionsPerMillisecond();
                output.WriteLine($"Average for {candidate.Name}: {candidate.GetAverageExecutionsPerMillisecond():N2} executions per ms ({ratioToBest:P2} of best)");
            }
        }
    }
}