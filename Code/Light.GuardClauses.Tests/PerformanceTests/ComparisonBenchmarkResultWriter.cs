using System.Collections.Generic;
using Xunit.Abstractions;

namespace Light.GuardClauses.Tests.PerformanceTests
{
    public class ComparisonBenchmarkResultWriter
    {
        public virtual void WriteResults(string testHeader,
                                         IList<ComparisonCandidate> orderedCandidates,
                                         IList<List<int>> passedValues,
                                         ITestOutputHelper output)
        {
            output.WriteLine(testHeader);
            output.WriteLine(new string('-', 40));
            output.WriteLine(string.Empty);

            for (var i = 0; i < passedValues.Count; i++)
            {
                var passedValue = passedValues[i];
                output.WriteLine(passedValue.ToString());

                foreach (var performanceCandidate in orderedCandidates)
                {
                    var testRunResult = performanceCandidate.Results[i];
                    output.WriteLine($"{performanceCandidate.Name}: {testRunResult.TotalMilliseconds:N0}ms");
                }
                output.WriteLine(string.Empty);
                output.WriteLine(string.Empty);
            }
        }
    }
}