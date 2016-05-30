using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace Light.GuardClauses.Tests.PerformanceTests
{
    public sealed class MustNotBeNullMacrobenchmark : BaseComparisonBenchmark
    {
        public MustNotBeNullMacrobenchmark(ITestOutputHelper output) : base(output) { }

        [Fact(DisplayName = "MustNotBeNull Macrobenchmark")]
        [Trait("Category", "PerformanceTest")]
        public void TestMethodName()
        {
            var testValues = CreateTestValues(200000, 400000, 8000000, 1600000);

            var candidates = new List<ComparisonCandidate>
                             {
                                 new ComparisonCandidate("Imperative Null Check", RunTestWithImperativeNullCheck),
                                 new ComparisonCandidate("Light.GuardClauses", RunTestWithLightGuardClauses),
                                 new ComparisonCandidate("Fluent Assertions", RunTestWithFluentAssertions)
                             };

            RunPerformanceTests("Null Check Macrobenchmark", candidates, testValues);
        }

        private TimeSpan RunTestWithLightGuardClauses(List<int> list)
        {
            Stopwatch.Start();

            list.MustNotBeNull(nameof(list));
            list.ToList().Sort();

            Stopwatch.Stop();

            return Stopwatch.Elapsed;
        }

        private TimeSpan RunTestWithImperativeNullCheck(List<int> list)
        {
            Stopwatch.Start();

            if (list == null)
                throw new ArgumentNullException(nameof(list));

            list.ToList().Sort();

            Stopwatch.Stop();
            return Stopwatch.Elapsed;
        }

        private TimeSpan RunTestWithFluentAssertions(List<int> list)
        {
            Stopwatch.Start();

            list.Should().NotBeNull();
            list.ToList().Sort();

            Stopwatch.Stop();
            return Stopwatch.Elapsed;
        }

        private static List<List<int>> CreateTestValues(params int[] numberOfElements)
        {
            var lists = new List<List<int>>();

            foreach (var count in numberOfElements)
            {
                var list = new List<int>();
                for (var i = 0; i < count; i++)
                {
                    list.Add(count - 1);
                }

                lists.Add(list);
            }

            return lists;
        }
    }
}