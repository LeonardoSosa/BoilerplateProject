﻿using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using BoilerplateProject.Entities.Products;

namespace BoilerplateProject.Products.Dto
{
    [AutoMapFrom(typeof(Product))]
    public class ProductDto : EntityDto<int>
    {
        public string Name { get; set; }
        public double Price { get; set; }
        public int StockAmount { get; set; }
        public bool IsActive { get; set; }
    }
}
