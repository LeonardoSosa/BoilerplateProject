using Abp.AutoMapper;
using Abp.Runtime.Validation;
using BoilerplateProject.Entities.Products;
using System.ComponentModel.DataAnnotations;

namespace BoilerplateProject.Products.Dto
{
    [AutoMapTo(typeof(Product))]
    public class CreateProductDto : ICustomValidate
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public double Price { get; set; }
        public int StockAmount { get; set; }
        [Required]
        public bool IsActive { get; set; }
        [Required]
        public CreateProductPictureDto ProductPicture { get; set; }

        public void AddValidationErrors(CustomValidationContext context)
        {
            if (Price < 0)
            {
                context.Results.Add(new ValidationResult("Minimum price required for a product is 0"));
            }
            if (StockAmount < 1)
            {
                context.Results.Add(new ValidationResult("Minimum stock amount required for a product is 1"));
            }
        }
    }
}
