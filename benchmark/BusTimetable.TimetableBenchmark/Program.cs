using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace BusTimetable.TimetableBenchmark
{
    class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<TimetableTest>();
        }
    }

    [MemoryDiagnoser]
    public class TimetableTest
    {
        //todo add benchmarks
    }
}
