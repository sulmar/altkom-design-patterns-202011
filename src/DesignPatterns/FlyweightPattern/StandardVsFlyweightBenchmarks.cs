using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlyweightPattern
{
    [RankColumn]
    [MemoryDiagnoser]
    public class StandardVsFlyweightBenchmarks
    {
        [Benchmark]
        public void Standard()
        {
            Game game = new Game(TreeFactory.Create());

            game.Play();
        }

        [Benchmark]
        public void Flyweight()
        {
            SolutionGame game = new SolutionGame(TreeConcretFactory.Create());

            game.Play();
        }
    }
}
