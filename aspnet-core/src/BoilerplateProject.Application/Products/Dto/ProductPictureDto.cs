using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using BoilerplateProject.Entities.Products;

namespace BoilerplateProject.Products.Dto
{
    [AutoMapFrom(typeof(ProductPicture))]
    public class ProductPictureDto : EntityDto<int>
    {
        public int ProductId { get; set; }
        public byte[] PictureContent { get; set; }
        public string ContentType { get; set; }
    }
}
