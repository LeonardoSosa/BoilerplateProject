using Abp.AutoMapper;
using BoilerplateProject.Entities.Orders;

namespace BoilerplateProject.Orders.Dto
{
    [AutoMapTo(typeof(OrderedProduct))]
    public class UpdateOrderedProductDto
    {
        public int ProductId { get; set; }
        public double UnitPrice { get; set; }
        public int Amount { get; set; }
    }
}
