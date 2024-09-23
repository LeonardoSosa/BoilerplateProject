using Abp.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using BoilerplateProject.Authorization.Users;
using System.Collections.Generic;
using BoilerplateProject.Entities.Products;

namespace BoilerplateProject.Entities.Orders
{
    [Table("Orders")]
    public class Order : Entity
    {
        public long UserId { get; private set; }
        public User User { get; private set; }
        public List<Product> Products { get; private set; } = [];
        public int TotalItems { get; private set; }
        public double Total { get; private set; }
        public List<OrderedProduct> OrderedProducts { get; } = [];

        public Order(long userId)
        {
            UserId = userId;
            TotalItems = 0;
            Total = 0.0;
        }

        public void UpdateProperties(long userId)
        {
            this.UserId = userId;
            this.TotalItems = 0;
            this.Total = 0;
            foreach (var item in this.OrderedProducts)
            {
                TotalItems += item.Amount;
                Total += item.Subtotal;
            }
        }

        // Use only when creating a new Order
        public void AddOrderedProduct(OrderedProduct input)
        {
            input.OrderId = Id;
            OrderedProducts.Add(input);
            TotalItems += input.Amount;
            Total += input.Subtotal;
        }
    }
}
