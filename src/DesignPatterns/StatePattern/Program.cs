using System;
using Stateless;

namespace StatePattern
{
    // dotnet add package Stateless

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello State Pattern!");

           
            TrafficLight trafficLight = new TrafficLight(new MyTrafficLightService());

            Console.WriteLine(trafficLight.Graph);

            while (true)
            {
                Console.WriteLine(trafficLight.State);

                trafficLight.Push();

                //Console.WriteLine(trafficLight.State);

                //trafficLight.Push();

                Console.WriteLine("Press any key to blink...");
                Console.ReadKey();

                trafficLight.Blink();

                Console.WriteLine(trafficLight.State);

            }

            // OrderTest();

            // LampTest();

        }

        private static void LampTest()
        {
            Lamp lamp = new Lamp();
            Console.WriteLine(lamp.State);

            lamp.Push();
            Console.WriteLine(lamp.State);

            lamp.Push();
            Console.WriteLine(lamp.State);

            lamp.Push();
            Console.WriteLine(lamp.State);
        }

        private static void OrderTest()
        {
            Order order = Order.Create();

            order.Completion();

            if (order.Status == OrderStatus.Completion)
            {
                order.Status = OrderStatus.Sent;
                Console.WriteLine("Your order was sent.");
            }

            order.Cancel();
        }
    }

    #region Models

    public class Order
    {
        public Order(string orderNumber)
        {
            Status = OrderStatus.Created;

            OrderNumber = orderNumber;
            OrderDate = DateTime.Now;
         
        }

        public DateTime OrderDate { get; set; }

        public string OrderNumber { get; set; }

        public OrderStatus Status { get; set; }

        private static int indexer;

        public static Order Create()
        {
            Order order = new Order($"Order #{indexer++}");

            if (order.Status == OrderStatus.Created)
            {
                Console.WriteLine("Thank you for your order");
            }

            return order;
        }

        public void Completion()
        {
            if (Status == OrderStatus.Created)
            {
                this.Status = OrderStatus.Completion;

                Console.WriteLine("Your order is in progress");
            }
        }

        public void Cancel()
        {
            if (this.Status == OrderStatus.Created || this.Status == OrderStatus.Completion)
            {
                this.Status = OrderStatus.Canceled;

                Console.WriteLine("Your order was cancelled.");
            }
        }

    }

    public enum OrderStatus
    {
        Created,
        Completion,
        Sent,
        Canceled,
        Done
    }

    public class Lamp
    {
        public LampState State => machine.State;

        private StateMachine<LampState, LampTrigger> machine;

        public Lamp()
        {
            machine = new StateMachine<LampState, LampTrigger>(LampState.Off);

            machine.Configure(LampState.Off)
                .Permit(LampTrigger.Push, LampState.On);

            machine.Configure(LampState.On)
                .Permit(LampTrigger.Push, LampState.Off);
        }

        public void Push() => machine.Fire(LampTrigger.Push);
    }

    public enum LampState
    {
        On,
        Light,
        Off
    }


    

    public enum LampTrigger
    {
        Push
    }

    #endregion


    public interface ITrafficLightService
    {
        void Green();
        void Red();
        void Yellow();
        void Blink();
    }

    public class MyTrafficLightService : ITrafficLightService
    {
        public void Blink()
        {
            Console.WriteLine("My Blink");
        }

        public void Green()
        {
            Console.WriteLine("My Green");
        }

        public void Red()
        {
            Console.WriteLine("My Red");
        }

        public void Yellow()
        {
            Console.WriteLine("My Yellow");
        }
    }

    public class TrafficLight
    {
        public string Name { get; set; }

        private StateMachine<TrafficLightState, TrafficLightTrigger> machine;

        private System.Timers.Timer timer;

        public TrafficLightState State => machine.State;


        public string Graph => Stateless.Graph.UmlDotGraph.Format(machine.GetInfo());


        public bool IsDay => DateTime.Now.TimeOfDay < TimeSpan.Parse("15:00");

        public TrafficLight(ITrafficLightService trafficLightService)
        {
            machine = new StateMachine<TrafficLightState, TrafficLightTrigger>(TrafficLightState.Red);            

            machine.Configure(TrafficLightState.Red)
                .OnEntry(()=>trafficLightService.Red())
                .OnEntry(()=>timer.Start(), "Start timer")
                .OnExit(()=>timer.Stop(), "Stop timer")                
                .PermitIf(TrafficLightTrigger.Timer, TrafficLightState.Green,    () => IsDay)
                .PermitIf(TrafficLightTrigger.Timer, TrafficLightState.Blinking, () => !IsDay)
                .Permit(TrafficLightTrigger.Manual, TrafficLightState.Green)
                .Permit(TrafficLightTrigger.Blink, TrafficLightState.Blinking);

            machine.Configure(TrafficLightState.Green)
                .OnEntry(()=>trafficLightService.Green())
                .OnEntry(() => timer.Start(), "Start timer")
                .OnExit(() => timer.Stop(), "Stop timer")
                .Permit(TrafficLightTrigger.Timer, TrafficLightState.Yellow)
                .Permit(TrafficLightTrigger.Manual, TrafficLightState.Red)
                .Permit(TrafficLightTrigger.Blink, TrafficLightState.Blinking);

            machine.Configure(TrafficLightState.Yellow)
                .OnEntry(() => timer.Start(), "Start timer")
                .OnExit(() => timer.Stop(), "Stop timer")
                .Permit(TrafficLightTrigger.Timer, TrafficLightState.Red)
                .Permit(TrafficLightTrigger.Blink, TrafficLightState.Blinking);

            machine.Configure(TrafficLightState.Blinking)
                .OnEntry(()=>timer.Stop())
                .Permit(TrafficLightTrigger.Manual, TrafficLightState.Red);

            machine.OnTransitioned(t => Console.WriteLine($"{t.Trigger} {t.Source} -> {t.Destination}"));

            timer = new System.Timers.Timer(TimeSpan.FromSeconds(3).TotalMilliseconds);
            
            timer.AutoReset = false;
            timer.Elapsed += Timer_Elapsed;

        }

        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            machine.Fire(TrafficLightTrigger.Timer);
        }

        public void Push() => machine.Fire(TrafficLightTrigger.Manual);

        public void Blink() => machine.Fire(TrafficLightTrigger.Blink);

        public bool CanPush => machine.CanFire(TrafficLightTrigger.Manual);




        


    }

    public enum TrafficLightState
    {
        Red,
        Yellow,
        Green,
        Blinking
    }

    public enum TrafficLightTrigger
    {
        Timer,
        Manual,
        Blink
    }

}
