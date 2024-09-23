using Abp.AutoMapper;
using Abp.Runtime.Validation;
using BoilerplateProject.Entities.Products;
using Microsoft.AspNetCore.Http;

namespace BoilerplateProject.Products.Dto
{
    [AutoMapTo(typeof(ProductPicture))]
    public class UpdateProductPictureDto
    {
        [DisableValidation]
        public IFormFile? PictureFile { get; set; }
    }
}
