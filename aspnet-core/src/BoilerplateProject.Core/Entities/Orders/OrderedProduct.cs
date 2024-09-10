using Abp.Domain.Entities;
using BoilerplateProject.Entities.Products;
using System.ComponentModel.DataAnnotations.Schema;

namespace BoilerplateProject.Entities.Orders
{
    [Table("OrderedProducts")]
    public class OrderedProduct : Entity
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public Order Order { get; set; }
        public Product Product { get; set; }
        public double UnitPrice { get; set; }
        public int Amount { get; set; }
        public double Subtotal { get; set; }   // UnitPrice * Amount

        public OrderedProduct(int productId, double unitPrice, int amount)
        {
            ProductId = productId;
            UnitPrice = unitPrice;
            Amount = amount;
            Subtotal = UnitPrice * Amount;
        }
    }
}
