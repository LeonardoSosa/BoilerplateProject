using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using BoilerplateProject.Entities.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoilerplateProject.Orders.Dto
{
    [AutoMapFrom(typeof(OrderedProduct))]
    public class OrderedProductDto : EntityDto<int>
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public double UnitPrice { get; set; }
        public int Amount { get; set; }
        public double Subtotal { get; set; }
    }
}
