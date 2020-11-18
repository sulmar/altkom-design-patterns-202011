using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace DRY
{

    public interface IEntityRepository<TEntity>
    {
        IEnumerable<TEntity> Get();
        TEntity Get(Guid id);
        void Add(TEntity entity);
        void Update(TEntity entity);
        void Remove(Guid id);       
    }

    public class FakeCustomerRepository : FakeEntityRepository<Customer>, ICustomerRepository
    {
        public IEnumerable<Customer> Get(CustomerSearchCriteria searchCriteria)
        {
            throw new NotImplementedException();
        }
    }


    public class FakeProductRepository : FakeEntityRepository<Product>, IProductRepository
    {
        public IEnumerable<Product> GetByColor(string color)
        {
            return entities.Where(p => p.Color == color).ToList();
        }
    }

    public class FakeServiceRepository : FakeEntityRepository<Service>, IServiceRepository
    {

    }

    public class FakeEntityRepository<TEntity> : IEntityRepository<TEntity>
        where TEntity : BaseEntity
    {
        protected readonly ICollection<TEntity> entities = new Collection<TEntity>();

        public void Add(TEntity entity)
        {
            entities.Add(entity);
        }

        public IEnumerable<TEntity> Get()
        {
            return entities;
        }

        public TEntity Get(Guid id)
        {
            return entities.SingleOrDefault(e => e.Id == id);
        }

        public void Remove(Guid id)
        {
            entities.Remove(Get(id));
        }

        public void Update(TEntity entity)
        {
            Remove(entity.Id);
            Add(entity);
        }

       
    }

    public interface ICustomerRepository : IEntityRepository<Customer>
    {
        IEnumerable<Customer> Get(CustomerSearchCriteria searchCriteria);
    }


    public interface IProductRepository : IEntityRepository<Product>
    {
        IEnumerable<Product> GetByColor(string color);
    }

    public interface IServiceRepository : IEntityRepository<Service>
    {

    }


    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            Customer customer = new Customer();

        }
    }

    public abstract class SearchCriteria : Base
    {

    }


    public class CustomerSearchCriteria : SearchCriteria
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
    }


    public abstract class Base
    {
        
    }

    public abstract class BaseEntity : Base
    {
        public Guid Id { get; set; }
    }

   
    public class Customer : BaseEntity
    {       
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Birthday { get; set; }
    }

    public class OrderDetail : BaseEntity
    {
        public SaleItem Item { get; set; }
        public int Quantity { get; set; }
    }

    public abstract class SaleItem : BaseEntity
    {
        public string Name { get; set; }
        public decimal UnitPrice { get; set; }
    }

    public class Product : SaleItem
    {
        public string Color { get; set; }
    }

    public class Service : SaleItem
    {
        public TimeSpan Duration { get; set; }
    }
}
