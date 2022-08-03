using Application;
using BenchmarkDotNet.Attributes;
using Bogus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationBenchmark
{
    [MemoryDiagnoser]
    [RankColumn]
    public class Evaluation
    {
        private LongestPassword _longestPassword;

        private string _passwords;

        [Benchmark]
        public int SolutionRegexWithOrderedSelect() => _longestPassword.SolutionRegexWithOrderedSelect(_passwords);

        [Benchmark]
        public int SolutionRegExWithMax() => _longestPassword.SolutionRegExWithMax(_passwords);

        [Benchmark]
        public int SolutionIsLetterOrDigit() => _longestPassword.SolutionIsLetterOrDigit(_passwords);

        [Benchmark]
        public int SolutionStrictIsLetterOrDigit() => _longestPassword.SolutionStrictIsLetterOrDigit(_passwords);

        [Benchmark]
        public int Solution() => _longestPassword.Solution(_passwords);


        [GlobalSetup]
        public void GlobalSetup()
        {
            // generate a bunch of passwords
            var faker = new Faker();
            var passwords = Enumerable.Range(1, 7)
              .Select(_ => faker.Internet.Password())
              .ToList();

            // join them together into a string
            _passwords = string.Join(' ', passwords);

            // setup the sut
            _longestPassword = new LongestPassword();
        }
    }
}
