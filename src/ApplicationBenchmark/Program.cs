// See https://aka.ms/new-console-template for more information
using ApplicationBenchmark;
using BenchmarkDotNet.Running;

var useSelect = BenchmarkRunner.Run<Evaluation>();

Console.ReadKey();
