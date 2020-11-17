using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace StrategyPattern
{

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello Strategy Pattern!");

            // HappyHoursOrderCalculatorTest();

            SmartHappyHoursOrderCalculatorTest();
        }

        private static void HappyHoursOrderCalculatorTest()
        {
            Customer customer = new Customer("Anna", "Kowalska");

            Order order = CreateOrder(customer);

            // HappyHoursOrderCalculator calculator = new HappyHoursOrderCalculator();

            IDiscountStrategy discountStrategy = new HappyHoursDiscountStrategy(TimeSpan.Parse("09:00"), TimeSpan.Parse("16:00"), 0.1m);

            discountStrategy = new GenderDiscountStrategy(Gender.Female, 0.2m);

            DiscountOrderCalculator calculator = new DiscountOrderCalculator(discountStrategy);

            decimal discount = calculator.CalculateDiscount(order);

            Console.WriteLine($"Original amount: {order.Amount:C2} Discount: {discount:C2}");
        }

        private static void SmartHappyHoursOrderCalculatorTest()
        {
            Customer customer = new Customer("Anna", "Kowalska");

            Order order = CreateOrder(customer);

            ICanDiscountStrategy canDiscountStrategy = new HappyHoursCanDiscountStrategy(TimeSpan.Parse("09:00"), TimeSpan.Parse("16:00"));
            ICalculateDiscountStrategy calculateDiscountStrategy = new PercentageCalculateDiscountStrategy(0.2m);

            SmartDiscountOrderCalculator calculator = new SmartDiscountOrderCalculator(canDiscountStrategy, calculateDiscountStrategy);

            decimal discount = calculator.CalculateDiscount(order);

            Console.WriteLine($"Original amount: {order.Amount:C2} Discount: {discount:C2}");
        }

        private static void PromotionTest()
        {
            ICanDiscountStrategy canDiscountStrategy = new HappyHoursCanDiscountStrategy(TimeSpan.Parse("09:00"), TimeSpan.Parse("16:00"));
            ICalculateDiscountStrategy calculateDiscountStrategy = new PercentageCalculateDiscountStrategy(0.2m);

            Promotion promotion = new Promotion
            {
                Id = Guid.NewGuid(),
                Name = "Happy Hours 2020",
                From = DateTime.Parse("2020-01-01"),
                To = DateTime.Parse("2020-12-31"),
                CanDiscountStrategy = canDiscountStrategy,
                CalculateDiscountStrategy = calculateDiscountStrategy
            };

            Customer customer = new Customer("Anna", "Kowalska");
            Order order = CreateOrder(customer);
            order.Promotion = promotion;

        }

        private static void GenderOrderCalculatorTest()
        {
            Customer customer = new Customer("Anna", "Kowalska");

            Order order = CreateOrder(customer);

            GenderOrderCalculator calculator = new GenderOrderCalculator();
            decimal discount = calculator.CalculateDiscount(order);

            Console.WriteLine($"Original amount: {order.Amount:C2} Discount: {discount:C2}");
        }

        private static Order CreateOrder(Customer customer)
        {
            Product product1 = new Product(1, "Książka C#", unitPrice: 100m);
            Product product2 = new Product(2, "Książka Praktyczne Wzorce projektowe w C#", unitPrice: 150m);
            Product product3 = new Product(3, "Zakładka do książki", unitPrice: 10m);

            Order order = new Order(DateTime.Parse("2020-06-12 14:59"), customer);
            order.AddDetail(product1);
            order.AddDetail(product2);
            order.AddDetail(product3, 5);

            return order;
        }
    }


    #region Models


    public class Promotion
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime From { get; set; }
        public DateTime? To { get; set; }
        
        public ICanDiscountStrategy CanDiscountStrategy { get; set; }   // -> serializacja do json
        public ICalculateDiscountStrategy CalculateDiscountStrategy { get; set; } // -> serializacja do json

        public string CanDiscountStrategyJson { get; set; }
    }

    public class Order
    {
        public DateTime OrderDate { get; set; }
        public Customer Customer { get; set; }
        public decimal Amount => Details.Sum(p => p.LineTotal);

        public ICollection<OrderDetail> Details = new Collection<OrderDetail>();

        public Promotion Promotion { get; set; }

        public void AddDetail(Product product, int quantity = 1)
        {
            OrderDetail detail = new OrderDetail(product, quantity);

            this.Details.Add(detail);
        }

        public Order(DateTime orderDate, Customer customer)
        {
            OrderDate = orderDate;
            Customer = customer;
        }
    }

    public class Product
    {
        public Product(int id, string name, decimal unitPrice)
        {
            Id = id;
            Name = name;
            UnitPrice = unitPrice;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public decimal UnitPrice { get; set; }
    }

    public class OrderDetail
    {
        public OrderDetail(Product product, int quantity = 1)
        {
            Product = product;
            Quantity = quantity;

            UnitPrice = product.UnitPrice;
        }

        public int Id { get; set; }
        public Product Product { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal LineTotal => UnitPrice * Quantity;
    }

    public class Customer
    {
        public Customer(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;

            if (firstName.EndsWith("a"))
            {
                Gender = Gender.Female;
            }
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Gender Gender { get; set; }

    }

    public enum Gender
    {
        Male,
        Female
    }

    #endregion
}
