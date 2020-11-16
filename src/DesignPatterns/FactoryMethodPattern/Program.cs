using System;

namespace FactoryMethodPattern
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello Factory Method Pattern!");

            VisitCalculateAmountTest();
        }

        private static void VisitCalculateAmountTest()
        {
            IVisitFactory visitFactory = new PolandVisitFactory();
            IColorFactory colorFactory = new ColorFactory();

            while (true)
            {
                Console.Write("Podaj rodzaj wizyty: (N)FZ (P)rywatna (F)irma: ");
                string visitType = Console.ReadLine();

                Console.Write("Podaj czas wizyty w minutach: ");
                if (double.TryParse(Console.ReadLine(), out double minutes))
                {
                    TimeSpan duration = TimeSpan.FromMinutes(minutes);

                    Visit visit = visitFactory.Create(visitType, duration);

                    decimal totalAmount = visit.CalculateCost();

                    Console.ForegroundColor = colorFactory.Create(totalAmount);

                    Console.WriteLine($"Total amount {totalAmount:C2}");

                    Console.ResetColor();
                }
            }

        }
    }

    // Abstract Factory
    public interface IColorFactory
    {
        ConsoleColor Create(decimal amount);
    }

   

    // Concrete Factory
    public class ColorFactory : IColorFactory
    {
        public ConsoleColor Create(decimal amount)
        {
            // match pattterns
            
            if (amount == 0)
                return ConsoleColor.Green;
            else
                 if (amount >= 500 && amount < 1000)
                return ConsoleColor.Blue;
            else
                if (amount >= 1000)
                return ConsoleColor.Red;       

            else
                return ConsoleColor.White;
        }
    }

    // Abstract Factory
    public interface IVisitFactory
    {
        Visit Create(string visitType, TimeSpan duration);
    }

    // Concrete Factory
    public class PolandVisitFactory : IVisitFactory
    {
        public Visit Create(string visitType, TimeSpan duration)
        {
            switch (visitType)
            {
                case "N": return new NfzVisit(duration);
                case "P": return new PrivateVisit(duration, 100);
                case "F": return new CompanyVisit(duration, 100);

                default: throw new NotSupportedException(visitType);
            }
        }
    }

    // Concrete Factory

    public class USAVisitFactory : IVisitFactory
    {
        public Visit Create(string visitType, TimeSpan duration)
        {
            switch (visitType)
            {                
                case "P": return new PrivateVisit(duration, 100);
                case "C": return new CompanyVisit(duration, 100);

                default: throw new NotSupportedException(visitType);
            }
        }
    }

    #region Models

    public class NfzVisit : Visit
    {
        public NfzVisit(TimeSpan duration) : base(duration)
        {
        }

        public override decimal CalculateCost()
        {
            return 0;
        }
    }

    public class PrivateVisit : Visit
    {
        public PrivateVisit(TimeSpan duration, decimal pricePerHour) : base(duration)
        {
            this.PricePerHour = pricePerHour;
        }

        public decimal PricePerHour { get; set; }

        public override decimal CalculateCost()
        {
            return (decimal)Duration.TotalHours * PricePerHour;
        }
    }

    public class CompanyVisit : Visit
    {
        public decimal PricePerHour { get; set; }

        private const decimal companyDiscountPercentage = 0.9m;

        public CompanyVisit(TimeSpan duration, decimal pricePerHour) : base(duration)
        {
            this.PricePerHour = pricePerHour;
        }

        public override decimal CalculateCost()
        {
            return (decimal)Duration.TotalHours * PricePerHour * companyDiscountPercentage;
        }
    }


    public abstract class Visit
    {
        public DateTime VisitDate { get; set; }
        public TimeSpan Duration { get; set; }
        
        public Visit(TimeSpan duration)
        {
            VisitDate = DateTime.Now;
            Duration = duration;            
        }

        public abstract decimal CalculateCost();
       
    }

    #endregion
}
