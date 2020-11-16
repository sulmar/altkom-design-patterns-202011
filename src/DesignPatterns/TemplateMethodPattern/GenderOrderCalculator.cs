using System;

namespace TemplateMethodPattern
{
    // Gender - 20% upustu dla kobiet
    public class GenderOrderCalculator
    {
        public decimal CalculateDiscount(Order order)
        {
            // 1. Predykat
            if (order.Customer.Gender == Gender.Female)
            {
                // 2. Wartosc rabatu
                return order.Amount * 0.2m;
            }
            else
                return 0;
        }
    }

    // Happy Hours - 10% upustu w godzinach od 9 do 15
    public class HappyHoursOrderCalculator
    {
        public decimal CalculateDiscount(Order order)
        {
            // 1. Predykat
            if (order.OrderDate.Hour >= 9 && order.OrderDate.Hour <= 15)
            {
                // 2. Wartosc rabatu
                return order.Amount * 0.1m;
            }
            else
                return 0;
        }
    }


    public class GenderDiscountOrderCalculator : DiscountOrderCalculator
    {
        private readonly Gender gender;
        private readonly decimal percentage;

        public GenderDiscountOrderCalculator(Gender gender = Gender.Female, decimal percentage = 0)
        {
            this.gender = gender;
            this.percentage = percentage;
        }

        public override bool CanDiscount(Order order) => order.Customer.Gender == gender;
        public override decimal Discount(Order order) => order.Amount * percentage;
    }

    public class HappyHoursDiscountOrderCalculator : DiscountOrderCalculator
    {
        private readonly TimeSpan from;
        private readonly TimeSpan to;
        private readonly decimal percentage;

        public HappyHoursDiscountOrderCalculator(TimeSpan from, TimeSpan to, decimal percentage)
        {
            this.from = from;
            this.to = to;
            this.percentage = percentage;
        }

        public override bool CanDiscount(Order order)
        {
            return order.OrderDate.TimeOfDay >= from && order.OrderDate.TimeOfDay <= to;
        }

        public override decimal Discount(Order order)
        {
            return order.Amount * percentage;
        }
    }

    // Template
    public abstract class DiscountOrderCalculator
    {
        public abstract bool CanDiscount(Order order);
        public abstract decimal Discount(Order order);

        private const decimal noDiscount = 0;

        // Template Method
        public decimal CalculateDiscount(Order order)
        {
            // 1. Predykat
            if (CanDiscount(order))
            {
                // 2. Wartosc rabatu
                return Discount(order);
            }
            else
                return noDiscount;
        }
    }
}
