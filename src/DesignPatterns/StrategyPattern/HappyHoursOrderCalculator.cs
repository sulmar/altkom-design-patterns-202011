using System;

namespace StrategyPattern
{
    // Happy Hours - 10% upustu w godzinach od 9 do 15
    public class HappyHoursOrderCalculator
    {
        public decimal CalculateDiscount(Order order)
        {
            if (order.OrderDate.Hour >= 9 && order.OrderDate.Hour <= 15)
            {
                return order.Amount * 0.1m;
            }
            else
                return 0;
        }
    }

    // Abstract strategy
    public interface IDiscountStrategy
    {
        bool CanDiscount(Order order);
        decimal CalculateDiscount(Order order);
    }


    // Separacja interfejsow

    public interface ICanDiscountStrategy
    {
        bool CanDiscount(Order order);
    }

    public interface ICalculateDiscountStrategy
    {
        decimal CalculateDiscount(Order order);
    }


    public class VisitCanDiscountStrategy : ICanDiscountStrategy

    {
        public bool CanDiscount(Order order)
        {
            throw new NotImplementedException();
        }
    }

    public class HappyHoursCanDiscountStrategy : ICanDiscountStrategy
    {
        private readonly TimeSpan from;
        private readonly TimeSpan to;

        public HappyHoursCanDiscountStrategy(TimeSpan from, TimeSpan to)
        {
            this.from = from;
            this.to = to;
        }

        public bool CanDiscount(Order order)
        {
            return order.OrderDate.TimeOfDay >= from && order.OrderDate.TimeOfDay <= to;
        }
    }

    public class GenderCanDiscountStrategy : ICanDiscountStrategy
    {
        private readonly Gender gender;

        public GenderCanDiscountStrategy(Gender gender)
        {
            this.gender = gender;
        }

        public bool CanDiscount(Order order)
        {
            return order.Customer.Gender == gender;
        }
    }

    public class PercentageCalculateDiscountStrategy : ICalculateDiscountStrategy
    {
        private readonly decimal percentage;

        public PercentageCalculateDiscountStrategy(decimal percentage)
        {
            this.percentage = percentage;
        }

        public decimal CalculateDiscount(Order order)
        {
            return order.Amount * percentage;
        }
    }

    public class FixedCalculateDiscountStrategy : ICalculateDiscountStrategy
    {
        private readonly decimal fixedAmount;

        public decimal CalculateDiscount(Order order)
        {
            if (order.Amount < fixedAmount)
                return order.Amount;
            else
                return fixedAmount;
        }
    }

    public class HappyHoursDiscountStrategy : IDiscountStrategy
    {
        private readonly TimeSpan from;
        private readonly TimeSpan to;
        private readonly decimal percentage;

        public HappyHoursDiscountStrategy(TimeSpan from, TimeSpan to, decimal percentage)
        {
            this.from = from;
            this.to = to;
            this.percentage = percentage;
        }

        public decimal CalculateDiscount(Order order)
        {
            return order.Amount * percentage;
        }

        public bool CanDiscount(Order order)
        {
            return order.OrderDate.TimeOfDay >= from && order.OrderDate.TimeOfDay <= to;
        }
    }

    public class GenderFixedDiscountStrategy : IDiscountStrategy
    {
        private readonly Gender gender;
        private readonly decimal fixedAmount;

        public GenderFixedDiscountStrategy(decimal fixedAmount, Gender gender = Gender.Male)
        {
            this.gender = gender;
            this.fixedAmount = fixedAmount;
        }

        public decimal CalculateDiscount(Order order)
        {
            if (order.Amount < fixedAmount)
                return order.Amount;
            else
                return fixedAmount;
        }

        public bool CanDiscount(Order order)
        {
            return order.Customer.Gender == gender;
        }
    }

    public class HappyHoursFixedDiscountStrategy : IDiscountStrategy
    {
        private readonly TimeSpan from;
        private readonly TimeSpan to;
        private readonly decimal fixedAmount;

        public HappyHoursFixedDiscountStrategy(TimeSpan from, TimeSpan to, decimal fixedAmount)
        {
            this.from = from;
            this.to = to;
            this.fixedAmount = fixedAmount;
        }

        public decimal CalculateDiscount(Order order)
        {
            if (order.Amount < fixedAmount)
                return order.Amount;
            else
                return fixedAmount;
        }

        public bool CanDiscount(Order order)
        {
            return order.OrderDate.TimeOfDay >= from && order.OrderDate.TimeOfDay <= to;
        }
    }

    public class GenderDiscountStrategy : IDiscountStrategy
    {
        private readonly Gender gender;
        private readonly decimal percentage;

        public GenderDiscountStrategy(Gender gender, decimal percentage)
        {
            this.gender = gender;
            this.percentage = percentage;
        }

        public decimal CalculateDiscount(Order order) => order.Amount * percentage;

        public bool CanDiscount(Order order) => order.Customer.Gender == gender;
    }

    public class DiscountOrderCalculator
    {
        private readonly IDiscountStrategy discountStrategy;

        private const decimal noDiscount = 0;

        public DiscountOrderCalculator(IDiscountStrategy discountStrategy)
        {
            this.discountStrategy = discountStrategy;
        }

        public decimal CalculateDiscount(Order order)
        {
            //1.Predykat
            if (discountStrategy.CanDiscount(order))
            {
                // 2. Wartosc rabatu
                return discountStrategy.CalculateDiscount(order);
            }
            else
                return noDiscount;
            }
    }

    public class SmartDiscountOrderCalculator
    {
        private readonly ICanDiscountStrategy canDiscountStrategy;
        private readonly ICalculateDiscountStrategy calculateDiscountStrategy;

        private const decimal noDiscount = 0;

        public SmartDiscountOrderCalculator(ICanDiscountStrategy canDiscountStrategy, ICalculateDiscountStrategy calculateDiscountStrategy)
        {
            this.canDiscountStrategy = canDiscountStrategy;
            this.calculateDiscountStrategy = calculateDiscountStrategy;
        }

        public decimal CalculateDiscount(Order order)
        {
            //1.Predykat
            if (canDiscountStrategy.CanDiscount(order))
            {
                // 2. Wartosc rabatu
                return calculateDiscountStrategy.CalculateDiscount(order);
            }
            else
                return noDiscount;
        }
    }

    // Template Method
    //public abstract class DiscountOrderCalculator
    //{
    //    public abstract bool CanDiscount(Order order);
    //    public abstract decimal Discount(Order order);

    //    private const decimal noDiscount = 0;

    //    // Template Method
    //    public decimal CalculateDiscount(Order order)
    //    {
    //        // 1. Predykat
    //        if (CanDiscount(order))
    //        {
    //            // 2. Wartosc rabatu
    //            return Discount(order);
    //        }
    //        else
    //            return noDiscount;
    //    }
    //}
}
