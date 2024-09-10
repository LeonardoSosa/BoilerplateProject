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

        // Use only when creating a new Order
        public void AddOrderedProduct(OrderedProduct input)
        {
            input.OrderId = Id;             // pega Id da entidade (Order) no backend, antes de salvar na DB
            OrderedProducts.Add(input);
            TotalItems += input.Amount;
            Total += input.Subtotal;
        }
    }
}
