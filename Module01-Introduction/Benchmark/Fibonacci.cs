using System.Collections.Generic;
using BenchmarkDotNet.Attributes;

namespace Dotnetos.AsyncExpert.Homework.Module01.Benchmark
{
    [DisassemblyDiagnoser(exportCombinedDisassemblyReport: true)]
    [MemoryDiagnoser()]
    public class Fibonacci
    {
        private static ulong[] Cache = new ulong[] { 0, 1, 1 };

        // HOMEWORK:
        // 1. Write implementations for RecursiveWithMemoization and Iterative solutions
        // 2. Add MemoryDiagnoser to the benchmark
        // 3. Run with release configuration and compare results
        // 4. Open disassembler report and compare machine code
        // 
        // You can use the discussion panel to compare your results with other students

        [Benchmark(Baseline = true)]
        [ArgumentsSource(nameof(Data))]
        public ulong Recursive(ulong n)
        {
            if (n == 1 || n == 2) return 1;
            return Recursive(n - 2) + Recursive(n - 1);
        }

        [Benchmark]
        [ArgumentsSource(nameof(Data))]
        public ulong RecursiveWithMemoization(ulong n)
        {
            if (n == 1 || n == 2) return 1;

            if (Cache.Length <= (int)n)
            {
                var oldCache = Cache;
                Cache = new ulong[n + 1];
                oldCache.CopyTo(Cache, 0);
            }

            return RecursiveWithMemoizationCache(n);
        }

        private ulong RecursiveWithMemoizationCache(ulong n)
        {
            ulong v;
            if ((v = Cache[n]) != 0ul) return v;

            var r = RecursiveWithMemoizationCache(n - 2) + RecursiveWithMemoizationCache(n - 1);
            Cache[n] = r;

            return r;
        }

        [Benchmark]
        [ArgumentsSource(nameof(Data))]
        public ulong Iterative(ulong n)
        {
            if (n == 1 || n == 2) return 1;

            ulong val1 = 1;
            ulong val2 = 1;
            for (ulong i = 3ul; i < n; i++)
            {
                ulong old2 = val2;
                val2 = val2 + val1;
                val1 = old2;
            }

            return val2 + val1;
        }

        public IEnumerable<ulong> Data()
        {
            yield return 15;
            yield return 35;
        }

        // * Summary *
        // BenchmarkDotNet=v0.12.1, OS=Windows 10.0.18363.1139 (1909/November2018Update/19H2)
        // Intel Core i7-7820HQ CPU 2.90GHz (Kaby Lake), 1 CPU, 8 logical and 4 physical cores
        // .NET Core SDK=3.1.201
        //   [Host]     : .NET Core 3.1.3 (CoreCLR 4.700.20.11803, CoreFX 4.700.20.12001), X64 RyuJIT
        //   DefaultJob : .NET Core 3.1.3 (CoreCLR 4.700.20.11803, CoreFX 4.700.20.12001), X64 RyuJIT
           
           
        // |                   Method |  n |              Mean |           Error |          StdDev | Ratio | Code Size | Gen 0 | Gen 1 | Gen 2 | Allocated |
        // |------------------------- |--- |------------------:|----------------:|----------------:|------:|----------:|------:|------:|------:|----------:|
        // |                Recursive | 15 |      2,492.953 ns |      27.3593 ns |      24.2533 ns | 1.000 |      76 B |     - |     - |     - |         - |
        // | RecursiveWithMemoization | 15 |          2.541 ns |       0.0439 ns |       0.0389 ns | 0.001 |     409 B |     - |     - |     - |         - |
        // |                Iterative | 15 |         12.712 ns |       0.1278 ns |       0.1133 ns | 0.005 |      65 B |     - |     - |     - |         - |
        // |                          |    |                   |                 |                 |       |           |       |       |       |           |
        // |                Recursive | 35 | 37,582,911.224 ns | 711,090.0832 ns | 630,362.8370 ns | 1.000 |      76 B |     - |     - |     - |         - |
        // | RecursiveWithMemoization | 35 |          2.512 ns |       0.0654 ns |       0.0546 ns | 0.000 |     409 B |     - |     - |     - |         - |
        // |                Iterative | 35 |         34.624 ns |       0.3892 ns |       0.3038 ns | 0.000 |      65 B |     - |     - |     - |         - |
    }
}
