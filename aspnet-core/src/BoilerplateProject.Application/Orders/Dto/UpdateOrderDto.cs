using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using BoilerplateProject.Entities.Orders;
using System.Collections.Generic;

namespace BoilerplateProject.Orders.Dto
{
    [AutoMapFrom(typeof(Order))]
    public class UpdateOrderDto : EntityDto<int>
    {
        public long UserId { get; set; }
        public List<UpdateOrderedProductDto> OrderedProducts { get; set; } = [];
    }
}
