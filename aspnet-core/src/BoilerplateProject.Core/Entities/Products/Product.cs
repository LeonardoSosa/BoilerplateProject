using Abp.Domain.Entities;
using BoilerplateProject.Entities.Orders;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
#nullable enable
namespace BoilerplateProject.Entities.Products
{
    [Table("Products")]
    public class Product : Entity, ISoftDelete
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public double Price { get; set; }
        [Required]
        public int StockAmount { get; set; }
        [Required]
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public ProductPicture? ProductPicture { get; set; }
        public List<Order> Orders { get; } = [];
        public List<OrderedProduct> OrderedProducts { get; } = [];
    }
}
#nullable disable
