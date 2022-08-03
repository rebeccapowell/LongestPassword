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
        public int UseSelect() => _longestPassword.SolutionSelect(_passwords);

        [Benchmark]
        public int UseMax() => _longestPassword.Solution(_passwords);

        [Benchmark]
        public int UseMaxBy() => _longestPassword.SolutionMaxBy(_passwords);

        [Benchmark]
        public int UseIsLetterOrDigit() => _longestPassword.SolutionIsLetterOrDigit(_passwords);

        [Benchmark]
        public int UseStrictIsLetterOrDigit() => _longestPassword.SolutionStrictIsLetterOrDigit(_passwords);

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
