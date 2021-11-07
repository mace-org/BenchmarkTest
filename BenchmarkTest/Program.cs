using BenchmarkDotNet.Running;
using System;

namespace BenchmarkTest
{
    class Program
    {
        static void Main(string[] args)
        {
            // BenchmarkRunner.Run<InvokeTest>();
            // BenchmarkRunner.Run<ExpressionTest>();
            BenchmarkRunner.Run<CreateInstanceTest>();
            Console.ReadLine();
        }
    }
}
