using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Abp.UI;
using BoilerplateProject.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BoilerplateProject.Entities.Orders
{
    public class OrderManager : DomainService
    {
        private readonly IRepository<Product> _productRepository;

        public OrderManager(IRepository<Product> productRepository)
        {
            _productRepository = productRepository;
        }

        public void Validate(Order order)
        {
            // instead of querying each product on each validation from repository
            // get all products that will be used on validation from the start
            var orderedIds =  order.OrderedProducts.Select(o => o.ProductId);
            var products = _productRepository.GetAll()
                .Where(p => orderedIds.Contains(p.Id)).ToList();

            List<string> errors = [];

            if (!DoExist(order.OrderedProducts, products)) {
                errors.Add("All products must exist");
            } else {
                if (!AreActive(order.OrderedProducts, products))
                {
                    errors.Add("All products must be active");
                }
                if (!SatisfyStock(order.OrderedProducts, products))
                {
                    errors.Add("The products' amount can't be greater than the stock");
                }
            }

            ThrowIfAny(errors);
        }

        protected bool DoExist(List<OrderedProduct> orderedProducts, List<Product> products)
        {
            foreach (var o in orderedProducts)
            {
                //var product = _productRepository.Get(o.ProductId);
                var product = products.Find(p => p.Id == o.ProductId);
                if (product == null)
                {
                    return false;
                }
            }
            return true;
        }

        protected bool AreActive(List<OrderedProduct> orderedProducts, List<Product> products)
        {
            foreach (var o in orderedProducts)
            {
                //var product = _productRepository.Get(o.ProductId);
                var product = products.Find(p => p.Id == o.ProductId);
                if (!product.IsActive)
                {
                    return false;
                }
            }
            return true;
        }

        protected bool SatisfyStock(List<OrderedProduct> orderedProducts, List<Product> products)
        {
            foreach (var o in orderedProducts)
            {
                //var product = _productRepository.Get(o.ProductId);
                var product = products.Find(p => p.Id == o.ProductId);
                if (o.Amount > product.StockAmount)
                {
                    return false;
                }
            }
            return true;
        }

        protected void ThrowIfAny(List<string> errors)
        {
            if (errors.Any())
            {
                throw new UserFriendlyException("The following errors where found during validation. " + 
                    String.Join(" - ", errors));
            }
        }
    }
}
