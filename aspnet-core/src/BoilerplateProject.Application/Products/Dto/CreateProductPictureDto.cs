using Abp.AutoMapper;
using BoilerplateProject.Entities.Products;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace BoilerplateProject.Products.Dto
{
    [AutoMapTo(typeof(ProductPicture))]
    public class CreateProductPictureDto
    {
        [Required]
        public IFormFile Picture {  get; set; }
    }
}
