using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using BoilerplateProject.Authorization.Users;
using BoilerplateProject.Entities.Orders;
using BoilerplateProject.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoilerplateProject.Orders.Dto
{
    [AutoMapFrom(typeof(Order))]
    public class OrderDto : EntityDto<int>
    {
        public long UserId { get; set; }
        public string Username { get; set; }
        public List<OrderedProductDto> OrderedProducts { get; set; } = [];
        public int TotalItems { get; set; }
        public double Total { get; set; }
    }
}
