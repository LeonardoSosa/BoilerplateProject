using Abp.AutoMapper;
using Abp.Localization;
using Abp.Runtime.Validation;
using BoilerplateProject.Entities.Orders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace BoilerplateProject.Orders.Dto
{
    [AutoMapTo(typeof(Order))]
    public class CreateOrderDto : ICustomValidate
    {
        public List<CreateOrderedProductDto> OrderedProducts { get; set; } = [];

        public void AddValidationErrors(CustomValidationContext context)
        {
            if (HasDuplicates(OrderedProducts))
            {
                context.Results.Add(new ValidationResult("All ordered products must be unique"));
            }
            if (!SatisfyAmount(OrderedProducts))
            {
                context.Results.Add(new ValidationResult("Minimum amount required for an ordered product is 1"));
            }
            if (!SatisfyUnitPrice(OrderedProducts))
            {
                context.Results.Add(new ValidationResult("Minimum price required for an ordered product is 0"));
            }
        }

        protected bool HasDuplicates(List<CreateOrderedProductDto> originalList)
        {
            return originalList
                .GroupBy(p => p.ProductId)
                .Any(g => g.Count() > 1);
        }

        protected bool SatisfyAmount(List<CreateOrderedProductDto> orderedProducts)
        {
            foreach (var o in orderedProducts)
            {
                if(o.Amount < 1)
                {
                    return false;
                }
            }
            return true;
        }

        protected bool SatisfyUnitPrice(List<CreateOrderedProductDto> orderedProducts)
        {
            foreach (var o in orderedProducts)
            {
                if (o.UnitPrice < 0)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
