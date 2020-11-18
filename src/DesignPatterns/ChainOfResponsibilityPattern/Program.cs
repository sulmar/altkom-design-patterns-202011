﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace ChainOfResponsibilityPattern
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello Chain of Responsibility Pattern!");

            Order order = new Order { TotalAmount = 1000 };

            if (order.TotalAmount>100) // kier
            {

            }
            else if (order.TotalAmount > 1000) // dyr
            {

            }
            else if (order.TotalAmount > 10000) // prezes
            {

            }



        }
    }


    public class Order
    {
        public decimal TotalAmount { get; set; }
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

        public override string ToString()
        {
            return $"{Id} {Name} {UnitPrice:C2}";
        }
    }

    public interface IProductService
    {
        Product Get(int id);
    }

    public class CacheProductService
    {
        private readonly IDictionary<int, Product> products = new Dictionary<int, Product>();

        public void Set(int id, Product product)
        {
            products.TryAdd(id, product);
        }

        public Product Get(int id)
        {
            Console.WriteLine($"Get product Id={id} from cache");

            products.TryGetValue(id, out Product product);

            return product;
        }


    }

    public class DbProductService : IProductService
    {
        private readonly ICollection<Product> products;

        public DbProductService()
        {
            products = new Collection<Product>
            {
                new Product(1, "Książka C#", 100m),
                new Product(2, "Książka Praktyczne Wzorce projektowe w C#", unitPrice: 150m),
                new Product(3, "Zakładka do książki", unitPrice: 10m),
            };
        }

        public Product Get(int id)
        {
            Console.WriteLine($"Get product Id={id} from database");
            return products.SingleOrDefault(p => p.Id == id);
        }
    }

}
