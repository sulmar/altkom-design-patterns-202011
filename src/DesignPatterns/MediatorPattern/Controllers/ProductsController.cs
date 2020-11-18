using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediatorPattern.Controllers
{
    
    public class ProductsController
    {
        private readonly DbContext context;

        public ProductsController(DbContext context)
        {
            this.context = context;
        }

        public void Post()
        {
            context.SaveChanges();
        }
    }
}
