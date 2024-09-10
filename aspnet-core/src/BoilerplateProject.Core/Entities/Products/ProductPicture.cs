using Abp.Domain.Entities;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace BoilerplateProject.Entities.Products
{
    [Table("ProductPictures")]
    public class ProductPicture : Entity
    {
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public byte[] PictureContent { get; set; }
        public string ContentType { get; set; }
    }
}
