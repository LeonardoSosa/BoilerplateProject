using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Abp.Domain.Services;
using BoilerplateProject.Authorization.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoilerplateProject.Entities.Products
{
    public class ProductManager : DomainService
    {
        private readonly IRepository<Product> _productRepository;

        public ProductManager(IRepository<Product> productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task CreateAsync(Product product)
        {
            // domain logic
        }

        public async Task DeleteAsync(int productId)
        {
            // domain logic
        }
    }
}
