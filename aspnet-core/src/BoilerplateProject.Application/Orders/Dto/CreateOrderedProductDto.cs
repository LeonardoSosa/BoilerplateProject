using Abp.AutoMapper;
using Abp.Runtime.Validation;
using BoilerplateProject.Entities.Orders;
using System.ComponentModel.DataAnnotations;

namespace BoilerplateProject.Orders.Dto
{
    [AutoMapTo(typeof(OrderedProduct))]
    public class CreateOrderedProductDto
    {
        public int ProductId { get; set; }
        public double UnitPrice { get; set; }
        public int Amount { get; set; }
    }
}
