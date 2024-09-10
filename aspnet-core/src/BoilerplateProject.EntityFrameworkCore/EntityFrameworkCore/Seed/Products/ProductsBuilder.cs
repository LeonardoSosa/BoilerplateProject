using BoilerplateProject.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoilerplateProject.EntityFrameworkCore.Seed.Products
{
    public class ProductsBuilder
    {
        private readonly BoilerplateProjectDbContext _context;

        public ProductsBuilder(BoilerplateProjectDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            CreateProducts();
        }

        private void CreateProducts()
        {
            // Products

            var product = _context.Products.FirstOrDefault();
            if (product == null)
            {
                product = new Product
                {
                    Name = "TestProduct",
                    Price = 05.1,
                    StockAmount = 15,
                    IsActive = true
                };

                _context.Products.Add(product);
                _context.SaveChanges();
            }
        }
    }
}
