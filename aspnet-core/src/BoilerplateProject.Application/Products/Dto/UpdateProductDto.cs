using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Runtime.Validation;
using BoilerplateProject.Entities.Products;
using System.ComponentModel.DataAnnotations;


namespace BoilerplateProject.Products.Dto
{
    [AutoMapFrom(typeof(Product))]
    public class UpdateProductDto : EntityDto<int>
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public double Price { get; set; }
        [Required]
        public int StockAmount { get; set; }
        [Required]
        public bool IsActive { get; set; }
        [DisableValidation]
        public UpdateProductPictureDto? ProductPicture { get; set; }
    }
}
