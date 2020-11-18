using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Reactive.Linq;
using System.Threading;

namespace ObserverPattern
{
    // Hot source

    // Cold source
    public class TempObservable : IObservable<float>
    {
        private ICollection<IObserver<float>> observers = new Collection<IObserver<float>>();

        public IDisposable Subscribe(IObserver<float> observer)
        {
            observers.Add(observer);

            float[] temperatures = new float[] { 0.6f, 12f, 5f, 7f, 30f };

            foreach (var temp in temperatures)
            {
                observer.OnNext(temp);
            }

            observer.OnCompleted();

            return null;

        }
    }

    public class ConsoleObserver : IObserver<float>
    {
        public void OnCompleted()
        {
            Console.WriteLine("End of transmission");
        }

        public void OnError(Exception error)
        {
            Console.WriteLine(error.Message);
        }

        public void OnNext(float value)
        {
            Console.WriteLine(value);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello Observer Pattern!");

            //       ObservableTest();

            //            Covid19Test();


            //              CpuTest();

            CpuObservableTest();

            Console.ReadKey();
        }

        private static void ObservableTest()
        {
            TempObservable source = new TempObservable();

            IObserver<float> observer = new ConsoleObserver();

            source.Subscribe(observer);
        }

        private static void Covid19Test()
        {
            IObservationService observationService = new FakeObservationService();

            var observations = observationService.Get();

            foreach (Observation observation in observations)
            {
                Console.WriteLine(observation);

                if (observation.Country == "Poland" && observation.Confirmed > 30)
                {
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Poland ALERT");
                    Console.ResetColor();
                }

                if (observation.Country == "Germany" && observation.Confirmed > 10)
                {
                    Console.BackgroundColor = ConsoleColor.Green;
                    Console.WriteLine($"Germany ALERT");
                    Console.ResetColor();
                }
            }
        }

        private static void CpuObservableTest()
        {
            var cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");

            // dotnet add package System.Reactive

            IObservable<float> source = Observable.Interval(TimeSpan.FromSeconds(1))
                .Select(_ => cpuCounter.NextValue());

            source.Subscribe(cpu => Console.WriteLine($"CPU {cpu}%"));

            IObservable<float> overLimitCpuSource = source.Where(cpu => cpu > 50);

            overLimitCpuSource.Subscribe(cpu =>
            {
                Console.BackgroundColor = ConsoleColor.Red;
                Console.WriteLine($"CPU {cpu} %");
                Console.ResetColor();
            });

            var bufferCpuSourceTime = source.Buffer(TimeSpan.FromSeconds(10));

            var bufferCpuSourceQty = source.Buffer(3);

            var buffer = bufferCpuSourceTime.Merge(bufferCpuSourceQty);



        }

        private static void CpuTest()
        {
            var cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");

            while (true)
            {
                float cpu = cpuCounter.NextValue();

                if (cpu < 30)
                {
                    Console.BackgroundColor = ConsoleColor.Green;
                    Console.WriteLine($"CPU {cpu} %");
                    Console.ResetColor();
                }
                else
                if (cpu > 50)
                {
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.WriteLine($"CPU {cpu} %");
                    Console.ResetColor();
                }
                else
                {
                    Console.WriteLine($"CPU {cpu} %");
                }

                Thread.Sleep(TimeSpan.FromSeconds(1));
            }
        }


        public class Observation
        {
            public string Country { get; set; }
            public int Confirmed { get; set; }
            public int Recovered { get; set; }
            public int Deaths { get; set; }

            public override string ToString()
            {
                return $"{Country} {Confirmed}/{Recovered}/{Deaths}";
            }

        }

        public interface IObservationService
        {
            IEnumerable<Observation> Get();
        }

        public class FakeObservationService : IObservationService
        {
            public IEnumerable<Observation> Get()
            {
                yield return new Observation { Country = "China", Confirmed = 2 };
                yield return new Observation { Country = "Germany", Confirmed = 1 };
                yield return new Observation { Country = "China", Confirmed = 20 };
                yield return new Observation { Country = "Germany", Confirmed = 60, Recovered = 4, Deaths = 2 };
                yield return new Observation { Country = "Poland", Confirmed = 10, Recovered = 5 };
                yield return new Observation { Country = "China", Confirmed = 30 };
                yield return new Observation { Country = "Poland", Confirmed = 50, Recovered = 15 };
                yield return new Observation { Country = "US", Confirmed = 10, Recovered = 5, Deaths = 1 };
                yield return new Observation { Country = "US", Confirmed = 11, Recovered = 3, Deaths = 4 };
                yield return new Observation { Country = "Poland", Confirmed = 45, Recovered = 25 };
                yield return new Observation { Country = "Germany", Confirmed = 52, Recovered = 4, Deaths = 1 };
            }
        }



    }



}
