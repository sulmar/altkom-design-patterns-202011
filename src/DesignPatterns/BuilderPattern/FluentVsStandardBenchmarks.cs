using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using System.Collections.Generic;

namespace BuilderPattern
{
    // dotnet add package BenchmarkDotNet

    [RankColumn]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    [MemoryDiagnoser]
    public class FluentVsStandardBenchmarks
    {
        [Benchmark]
        public void Fluent()
        {
            FluentPhone
              .Hangup()
              .From("555777555")
              .To("555999111")
              .WithSubject(".Design Pattern")
              .Call();

        }

        [Benchmark]
        public void Standard()
        {
            FluentPhone p = FluentPhone.Hangup();
            p.From("6546745645");
            p.To("555999111");
            p.WithSubject(".Design Pattern");
            p.Call();
        }

    }

    


}