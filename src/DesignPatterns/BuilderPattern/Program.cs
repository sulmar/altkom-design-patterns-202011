﻿using BenchmarkDotNet.Running;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml;

namespace BuilderPattern
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello Builder Pattern!");

            SalesReportTest();

            PhoneTest();

            FluentPhone
                .Hangup()
                .From("555777555")
                .To("555999111")
                .To("555666666")
                .WithSubject(".Design Pattern")
                .Call();

            //FluentPhone p = FluentPhone.Hangup();
            //p.From("6546745645");
            //p.Call();

            // Porownanie wydajnosci
            // dotnet run -c Release
            var summary = BenchmarkRunner.Run<FluentVsStandardBenchmarks>();

        }

        private static void SalesReportTest()
        {
            bool hasSectionByGender = true;

            FakeOrdersService ordersService = new FakeOrdersService();
            IEnumerable<Order> orders = ordersService.Get();

            ISalesReportBuilder salesReportBuilder = new MySalesReportBuilder(orders);

            salesReportBuilder.AddHeader();

            if (hasSectionByGender)
            {
                salesReportBuilder.AddSectionByGender();
            }

            salesReportBuilder.AddSectionByProduct();

            SalesReport salesReport = salesReportBuilder.Build();
                
            Console.WriteLine(salesReport);

        }

        private static void PhoneTest()
        {
            Phone phone = new Phone();
            phone.Call("555999123", "555000321", ".NET Design Patterns");
        }

       
    }

    #region SalesReport

    public class FakeOrdersService
    {
        private readonly IList<Product> products;
        private readonly IList<Customer> customers;

        public FakeOrdersService()
            : this(CreateProducts(), CreateCustomers())
        {

        }

        public FakeOrdersService(IList<Product> products, IList<Customer> customers)
        {
            this.products = products;
            this.customers = customers;
        }

        private static IList<Customer> CreateCustomers()
        {
            return new List<Customer>
            {
                 new Customer("Anna", "Kowalska"),
                 new Customer("Jan", "Nowak"),
                 new Customer("John", "Smith"),
            };

        }

        private static IList<Product> CreateProducts()
        {
            return new List<Product>
            {
                new Product(1, "Książka C#", unitPrice: 100m),
                new Product(2, "Książka Praktyczne Wzorce projektowe w C#", unitPrice: 150m),
                new Product(3, "Zakładka do książki", unitPrice: 10m),
            };
        }

        public IEnumerable<Order> Get()
        {
            Order order1 = new Order(DateTime.Parse("2020-06-12 14:59"), customers[0]);
            order1.AddDetail(products[0], 2);
            order1.AddDetail(products[1], 2);
            order1.AddDetail(products[2], 3);

            yield return order1;

            Order order2 = new Order(DateTime.Parse("2020-06-12 14:59"), customers[1]);
            order2.AddDetail(products[0], 2);
            order2.AddDetail(products[1], 4);

            yield return order2;

            Order order3 = new Order(DateTime.Parse("2020-06-12 14:59"), customers[2]);
            order2.AddDetail(products[0], 2);
            order2.AddDetail(products[2], 5);

            yield return order3;


        }
    }


    // Abstract builder
    public interface ISalesReportBuilder
    {
        void AddHeader();
        void AddSectionByGender();
        void AddSectionByProduct();
        void AddFooter();
        SalesReport Build();
    }

    // Concrete builder
    public class LazySalesReportBuilder : ISalesReportBuilder
    {
        private IEnumerable<Order> orders;

        private bool hasHeader;
        private bool hasSectionByGender;
        private bool hasFooter;

        public LazySalesReportBuilder(IEnumerable<Order> orders)
        {
            this.orders = orders;
        }

        public void AddFooter()
        {
            hasFooter = true;
        }

        public void AddHeader()
        {
            hasHeader = true;
        }

        public void AddSectionByGender()
        {
            hasSectionByGender = true;
        }

        public void AddSectionByProduct()
        {
            throw new NotImplementedException();
        }

        public SalesReport Build()
        {
            SalesReport salesReport = new SalesReport();

            if (hasHeader)
            {
                CreateHeader(salesReport);
            }

            if (hasSectionByGender)
            {

            }

            return salesReport;

        }

        private void CreateHeader(SalesReport salesReport)
        {
            salesReport.Title = "Raport sprzedaży";
            salesReport.CreateDate = DateTime.Now;
            salesReport.TotalSalesAmount = orders.Sum(s => s.Amount);
        }
    }


    public class MySalesReportBuilder : ISalesReportBuilder
    {
        private IEnumerable<Order> orders;

        private SalesReport salesReport;

        public MySalesReportBuilder(IEnumerable<Order> orders)
        {
            this.orders = orders;

            this.salesReport = new SalesReport();
        }

        public void AddHeader()
        {
            salesReport.Title = "Raport sprzedaży";
            salesReport.CreateDate = DateTime.Now;
            salesReport.TotalSalesAmount = orders.Sum(s => s.Amount);
        }
     
        public void AddSectionByGender()
        {
            salesReport.GenderDetails = orders
                    .GroupBy(o => o.Customer.Gender)
                    .Select(g => new GenderReportDetail(
                                g.Key,
                                g.Sum(x => x.Details.Sum(d => d.Quantity)),
                                g.Sum(x => x.Details.Sum(d => d.LineTotal))));
        }

        public void AddSectionByProduct()
        {
            salesReport.ProductDetails = orders
                .SelectMany(o => o.Details)
                .GroupBy(o => o.Product)
                .Select(g => new ProductReportDetail(g.Key, g.Sum(p => p.Quantity), g.Sum(p => p.LineTotal)));
        }

        public void AddFooter()
        {
            throw new NotImplementedException();
        }

        public SalesReport Build()
        {
            return salesReport;
        }
    }

    public class SalesReport
    {
        public string Title { get; set; }
        public DateTime CreateDate { get; set; }
        public decimal TotalSalesAmount { get; set; }

        public IEnumerable<ProductReportDetail> ProductDetails { get; set; }
        public IEnumerable<GenderReportDetail> GenderDetails { get; set; }


        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendLine("------------------------------");

            builder.AppendLine($"{Title} {CreateDate}");
             builder.AppendLine($"Total Sales Amount: {TotalSalesAmount:c2}");

             builder.AppendLine("------------------------------");

             builder.AppendLine("Total By Products:");
            foreach (var detail in ProductDetails)
            {
                 builder.AppendLine($"- {detail.Product.Name} {detail.Quantity} {detail.TotalAmount:c2}");
            }
             builder.AppendLine("Total By Gender:");
            foreach (var detail in GenderDetails)
            {
                 builder.AppendLine($"- {detail.Gender} {detail.Quantity} {detail.TotalAmount:c2}");
            }

            return builder.ToString();
        }
    }

    public class ProductReportDetail
    {
        public ProductReportDetail(Product product, int quantity, decimal totalAmount)
        {
            Product = product;
            Quantity = quantity;
            TotalAmount = totalAmount;
        }

        public Product Product { get; set; }
        public decimal TotalAmount { get; set; }
        public int Quantity { get; set; }
    }

    public class GenderReportDetail
    {
        public GenderReportDetail(Gender gender, int quantity, decimal totalAmount)
        {
            Gender = gender;
            Quantity = quantity;
            TotalAmount = totalAmount;
        }

        public Gender Gender { get; set; }
        public decimal TotalAmount { get; set; }
        public int Quantity { get; set; }
    }


    public class Order
    {
        public DateTime OrderDate { get; set; }
        public Customer Customer { get; set; }
        public decimal Amount => Details.Sum(p => p.LineTotal);

        public ICollection<OrderDetail> Details = new Collection<OrderDetail>();

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

    #region Phone

    public interface IFrom
    {
        ITo From(string number);
    }

    public interface IToOrCall : ITo, ICall, ISubject
    {
    }

    public interface ITo
    {
        IToOrCall To(string number);
    }

    public interface ISubject
    {
        ICall WithSubject(string subject);
    }

    public interface ICall
    {
        void Call();
    }

    // Builder w wersji FluentAPI
    public class FluentPhone : IFrom, ITo, ICall, ISubject, IToOrCall
    {
        private string from;

        public Collection<string> tos { get; set; } = new Collection<string>();

        private string subject;

        public static IFrom Hangup()
        {
            return new FluentPhone();
        }

        public ITo From(string number)
        {
            this.from = number;
            return this;
        }

        public IToOrCall To(string number)
        {
            this.tos.Add(number);
            return this;
        }

        public ICall WithSubject(string subject)
        {
            this.subject = subject;
            return this;
        }

        // Build
        public void Call()
        {
            foreach (var to in tos)
            {
                if (!string.IsNullOrEmpty(subject))
                {

                    Call(from, to, subject);
                }
                else
                {
                    Call(from, to);
                }
            }
            
        }

        private void Call(string from, string to, string subject)
        {
            Console.WriteLine($"Calling from {from} to {to} with subject {subject}");
        }

        private void Call(string from, string to)
        {
            Console.WriteLine($"Calling from {from} to {to}");
        }


    }

    public class Phone
    {
        public void Call(string from, string to, string subject)
        {
            Console.WriteLine($"Calling from {from} to {to} with subject {subject}");
        }

        public void Call(string from, string to)
        {
            Console.WriteLine($"Calling from {from} to {to}");
        }

        public void Call(string from, IEnumerable<string> tos, string subject)
        {
            foreach (var to in tos)
            {
                Call(from, to, subject);
            }
        }
    }

    #endregion


}