using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace NullObjectPattern
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello Null Object Pattern!");

            IProductRepository productRepository = new FakeProductRepository();

            ProductBase product = productRepository.Get(9);

            // Problem: Zawsze musimy sprawdzać czy obiekt nie jest pusty (null).

            //if (product != null)
            //{
                product.RateId(3);
            //}
        }
    }

    public interface IProductRepository
    {
        ProductBase Get(int id);
    }

    public class FakeProductRepository : IProductRepository
    {
        private readonly IEnumerable<ProductBase> products = new Collection<ProductBase>();

        public FakeProductRepository()
        {
            products = new Collection<ProductBase>
            {
                new Product { Id = 1 },
                new Product { Id = 2 },
                new Product { Id = 3 },
            };
        }

        public ProductBase Get(int id)
        {
            ProductBase product = products.SingleOrDefault(p=> p.Id == id);

            if (product == null)
                product = new NullProduct();

            return product;
        }
    }

    public abstract class ProductBase
    {
        public int Id { get; set; }
        public abstract void RateId(int rate);
    }

    public class Product : ProductBase
    {
       
        private int rate;

        public override void RateId(int rate)
        {
            this.rate = rate;
        }

    }

    // NullObject
    public class NullProduct : ProductBase
    {
        public override void RateId(int rate)
        {
            // nic nie rob
        }
    }
}
