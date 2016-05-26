using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace Light.GuardClauses.Tests
{
    public sealed class PerformanceTests
    {
        private readonly ITestOutputHelper _testOutput;
        private readonly Stopwatch _stopwatch = new Stopwatch();
        private readonly Timer _timer;
        private volatile bool _continue = true;

        private static readonly List<TimeSpan> PerformanceRunLengths = new List<TimeSpan>
                                                                       {
                                                                           TimeSpan.FromMilliseconds(100),
                                                                           TimeSpan.FromMilliseconds(400),
                                                                           TimeSpan.FromMilliseconds(700),
                                                                           TimeSpan.FromMilliseconds(1000),
                                                                           TimeSpan.FromMilliseconds(2000),
                                                                           TimeSpan.FromMilliseconds(3000)
                                                                       };


        public PerformanceTests(ITestOutputHelper testOutput)
        {
            _testOutput = testOutput;
            _timer = new Timer(StopPerformanceRun);
        }

        private void StopPerformanceRun(object state)
        {
            _continue = false;
        }

        [Fact(DisplayName = "MustNotBeNull Performance Test - at least 20% as fast as imperative version")]
        [Trait("Category", "PerformanceTest")]
        public void MustNotBeNullPerformaceTests()
        {
            var imperativeResults = new List<PerformanceTestRunResult>();
            var lightGuardClausesResults = new List<PerformanceTestRunResult>();
            var @object = new object();

            foreach (var performanceRunLength in PerformanceRunLengths)
            {
                var result = CheckForNullImperatively(@object, performanceRunLength);
                imperativeResults.Add(result);

                Reset();

                result = CheckForNullWithLightGuardClauses(@object, performanceRunLength);
                lightGuardClausesResults.Add(result);

                Reset();
            }

            OutputResults("MustNotBeNull Performance Test", imperativeResults, lightGuardClausesResults);
            OutputAverage(imperativeResults, lightGuardClausesResults, 0.2);
        }

        private void OutputAverage(List<PerformanceTestRunResult> imperativeResults, List<PerformanceTestRunResult> lightGuardClausesResults, double expectedRatio)
        {
            var imperativeAverage = imperativeResults.Select(r => r.NumberOfCalls / r.ElapsedTime.TotalMilliseconds)
                                                     .Average();
            var lightGuardClausesAverage = lightGuardClausesResults.Select(r => r.NumberOfCalls / r.ElapsedTime.TotalMilliseconds)
                                                                   .Average();

            var ratio = lightGuardClausesAverage / imperativeAverage;

            _testOutput.WriteLine($"Average imperative:         {imperativeAverage:N2} executions per ms");
            _testOutput.WriteLine($"Average Light.GuardClauses: {lightGuardClausesAverage:N2} executions per ms");
            _testOutput.WriteLine($"Ratio Light.GuardClauses to imperative: {ratio:P}");

            ratio.Should().BeGreaterThan(expectedRatio);
        }

        private PerformanceTestRunResult CheckForNullImperatively(object @object, TimeSpan lengthOfTestRun)
        {
            var numberOfLoopRuns = 0UL;
            _timer.Change(lengthOfTestRun, TimeSpan.FromMilliseconds(-1));
            _stopwatch.Start();

            while (_continue)
            {
                if (@object == null)
                    throw new ArgumentNullException(nameof(@object));

                numberOfLoopRuns++;
            }
            _stopwatch.Stop();

            return new PerformanceTestRunResult(numberOfLoopRuns, _stopwatch.Elapsed);
        }

        private PerformanceTestRunResult CheckForNullWithLightGuardClauses(object @object, TimeSpan lengthOfTestRun)
        {
            var numberOfLoopRuns = 0UL;
            _timer.Change(lengthOfTestRun, TimeSpan.FromMilliseconds(-1));
            _stopwatch.Start();

            while (_continue)
            {
                @object.MustNotBeNull(nameof(@object));

                numberOfLoopRuns++;
            }
            _stopwatch.Stop();

            return new PerformanceTestRunResult(numberOfLoopRuns, _stopwatch.Elapsed);
        }

        private void Reset()
        {
            _continue = true;
            _stopwatch.Reset();
        }

        private void OutputResults(string header, List<PerformanceTestRunResult> imperativeResult, List<PerformanceTestRunResult> lightGuardClausesResults)
        {
            _testOutput.WriteLine(header);
            _testOutput.WriteLine(new string('-', 40));
            _testOutput.WriteLine(string.Empty);

            for (var i = 0; i < PerformanceRunLengths.Count; i++)
            {
                var length = PerformanceRunLengths[i];
                _testOutput.WriteLine($"{length.TotalMilliseconds}ms test run:");
                _testOutput.WriteLine($"Imperative version: {imperativeResult[i].NumberOfCalls:N0} executions (in {imperativeResult[i].ElapsedTime.TotalMilliseconds:N0}ms)");
                _testOutput.WriteLine($"Light.GuardClauses: {lightGuardClausesResults[i].NumberOfCalls:N0} executions (in {lightGuardClausesResults[i].ElapsedTime.TotalMilliseconds:N0}ms)");
                _testOutput.WriteLine(string.Empty);
                _testOutput.WriteLine(string.Empty);
            }
        }

        private struct PerformanceTestRunResult
        {
            public readonly ulong NumberOfCalls;

            public readonly TimeSpan ElapsedTime;

            public PerformanceTestRunResult(ulong numberOfCalls, TimeSpan elapsedTime)
            {
                NumberOfCalls = numberOfCalls;
                ElapsedTime = elapsedTime;
            }
        }
    }
}